using RemoteX.Bluetooth.Rfcomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using System.IO;
using System.Net.Sockets;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Storage.Streams;
using Windows.Devices.Bluetooth;

namespace RemoteX.Bluetooth.Win10.Rfcomm
{
    internal class RXRfcommServiceProvider : IRfcommServiceProvider
    {
        public RfcommServiceProvider Win10RfcommServiceProvider { get; private set; }
        public StreamSocketListener RfcommServiceProviderSocketListener { get; }

        public IBluetoothManager BluetoothManager { get; }

        public event EventHandler<IRfcommConnection> OnConnectionReceived;

        public List<RXRFCommConnection> RfcommConnectionList;

        public Guid ServiceId
        {
            get
            {
                return Win10RfcommServiceProvider.ServiceId.Uuid;
            }
        }

        public IRfcommConnection[] Connections
        {
            get
            {
                return (from connection in RfcommConnectionList
                        select connection as IRfcommConnection).ToArray();
            }
        }

        public RXRfcommServiceProvider(IBluetoothManager bluetoothManager, RfcommServiceProvider win10Provider)
        {
            BluetoothManager = bluetoothManager;
            Win10RfcommServiceProvider = win10Provider;
            RfcommConnectionList = new List<RXRFCommConnection>();
            RfcommServiceProviderSocketListener = new StreamSocketListener();
            RfcommServiceProviderSocketListener.ConnectionReceived += RfcommServiceProviderSocketListener_ConnectionReceived;
            RfcommServiceProviderSocketListener.BindServiceNameAsync("{" + ServiceId.ToString().ToUpper() + "}", SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication).AsTask().Wait();
            InitializeServiceSdpAttributes(Win10RfcommServiceProvider, "TestService");
        }

        private async void RfcommServiceProviderSocketListener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            StreamSocket socket = null;
            socket = args.Socket;
            var remoteWin10Device = await BluetoothDevice.FromHostNameAsync(socket.Information.RemoteHostName);
            var remoteDevice = (BluetoothManager as BluetoothManager).GetBluetoothDeviceFromDeviceInformation(remoteWin10Device.DeviceInformation);
            System.Diagnostics.Debug.WriteLine("SOME ONE CONNECT:" + remoteWin10Device.DeviceInformation.Id);
            var connection = new RXRFCommConnection(this, remoteDevice, socket);
            RfcommConnectionList.Add(connection);
            OnConnectionReceived?.Invoke(this, connection);
        }

        

        private void InitializeServiceSdpAttributes(RfcommServiceProvider rfcommProvider, string serviceName)
        {
            byte SdpServiceNameAttributeType = (4 << 3) | 5;
            UInt16 SdpServiceNameAttributeId = 0x100;
            var sdpWriter = new DataWriter();

            // Write the Service Name Attribute.
            sdpWriter.WriteByte(SdpServiceNameAttributeType);

            // The length of the UTF-8 encoded Service Name SDP Attribute.
            sdpWriter.WriteByte((byte)serviceName.Length);

            // The UTF-8 encoded Service Name value.
            sdpWriter.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            sdpWriter.WriteString(serviceName);

            // Set the SDP Attribute on the RFCOMM Service Provider.
            rfcommProvider.SdpRawAttributes.Add(SdpServiceNameAttributeId, sdpWriter.DetachBuffer());
        }

        public void StartAdvertising()
        {

            Win10RfcommServiceProvider.StartAdvertising(RfcommServiceProviderSocketListener);
        }
    }
}
