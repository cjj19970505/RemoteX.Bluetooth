using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using RemoteX.Bluetooth.Rfcomm;
using static RemoteX.Bluetooth.Droid.BluetoothManager;

namespace RemoteX.Bluetooth.Droid.Rfcomm
{
    internal class RfcommServiceProvider : IRfcommServiceProvider
    {
        private List<IRfcommConnection> _Connections;
        public IRfcommConnection[] Connections
        {
            get
            {
                return _Connections.ToArray();
            }
        }

        public IBluetoothManager BluetoothManager { get; }

        public Guid ServiceId { get; }

        public event EventHandler<IRfcommConnection> OnConnectionReceived;
        public Android.Bluetooth.BluetoothServerSocket ServerSocket { get; private set; }
        Task AcceptTask;

        public RfcommServiceProvider(IBluetoothManager bluetoothManager, Guid serviceId)
        {
            BluetoothManager = bluetoothManager;
            ServiceId = serviceId;
            _Connections = new List<IRfcommConnection>();
        }
        public void StartAdvertising()
        {
            Task.Run(() =>
            {
                ServerSocket = (BluetoothManager as BluetoothManager).BluetoothAdapter.ListenUsingInsecureRfcommWithServiceRecord("RXService", UUID.FromString(ServiceId.ToString()));
                AcceptTask = RunAcceptTask();
            });
        }

        public Task RunAcceptTask()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    var bluetoothSocket = ServerSocket.Accept();
                    if (bluetoothSocket != null)
                    {
                        var rxDevice = BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice(BluetoothManager as BluetoothManager, bluetoothSocket.RemoteDevice);
                        RfcommConnection rfcommConnection = new RfcommConnection(rxDevice, bluetoothSocket);
                        _Connections.Add(rfcommConnection);
                        OnConnectionReceived?.Invoke(this, rfcommConnection);
                    }
                }
            });
        }
    }
}