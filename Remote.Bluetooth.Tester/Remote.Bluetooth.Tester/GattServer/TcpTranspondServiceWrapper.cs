using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading;

namespace Remote.Bluetooth.Tester.GattServer
{
    
    public class TcpTranspondServiceWrapper
    {
        public enum TcpConnState { Created, Connected, Connecting, Error}

        public IGattServerService GattServerService { get; private set; }
        public TranspondCharacteristicWrapper TranspondCharacteristicWrapper { get; private set; }
        public IBluetoothManager BluetoothManager { get; }
        public Guid Uuid = BluetoothUtils.ShortValueUuid(0x8888);
        public TcpClient TcpClient { get; set; }
        public IPEndPoint IPEndPoint { get; set; }
        public event EventHandler<TcpConnState> StateChanged;
        public event EventHandler<byte[]> MessageReceived;
        public TcpConnState State { get; private set; }
        private bool _CancelConnect = false;
        private Task _ReceiveTask;

        public NetworkStream NetworkStream;
        

        public TcpTranspondServiceWrapper(IBluetoothManager bluetoothManager)
        {
            BluetoothManager = bluetoothManager;
            TranspondCharacteristicWrapper = new TranspondCharacteristicWrapper(bluetoothManager);
            IGattServiceBuilder builder = bluetoothManager.NewGattServiceBuilder();
            GattServerService = builder.SetUuid(Uuid)
                                    .AddCharacteristics(TranspondCharacteristicWrapper.GattServerCharacteristic)
                                    .Build();
            TranspondCharacteristicWrapper.GattServerCharacteristic.OnRead += GattServerCharacteristic_OnRead;
            TranspondCharacteristicWrapper.GattServerCharacteristic.OnWrite += GattServerCharacteristic_OnWrite;
            State = TcpConnState.Created;
            TcpClient = new TcpClient();
        }

        private async void GattServerCharacteristic_OnWrite(object sender, ICharacteristicWriteRequest e)
        {
            await SendAsync(e.Value);
            e.RespondSuccess();
        }

        private void GattServerCharacteristic_OnRead(object sender, ICharacteristicReadRequest e)
        {
            
            e.RespondWithValue(TranspondCharacteristicWrapper.Value);
        }

        /// <summary>
        /// Will Retry connect until you cancel it
        /// </summary>
        public async Task ConnectAsync()
        {
            await Task.Run(async () =>
            {
                if (State != TcpConnState.Created)
                {
                    throw new NotImplementedException("WTF");
                }
                State = TcpConnState.Connecting;
                StateChanged?.Invoke(this, State);
                bool successful = false;
                while (!successful && !_CancelConnect)
                {
                    try
                    {
                        await TcpClient.ConnectAsync(IPEndPoint.Address, IPEndPoint.Port);
                        successful = true;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        continue;
                    }
                }
                if (_CancelConnect)
                {
                    State = TcpConnState.Created;
                    TcpClient = new TcpClient();
                    _CancelConnect = false;
                    StateChanged?.Invoke(this, State);
                }
                else if (successful)
                {
                    State = TcpConnState.Connected;
                    NetworkStream = TcpClient.GetStream();
                    StateChanged?.Invoke(this, State);
                    _ReceiveTask = _ReceiveAsync();
                }
            });
            
        }
        public void CancelConnect()
        {
            _CancelConnect = true;
        }

        private async Task _ReceiveAsync()
        {
            await Task.Run(async () =>
            {
                bool error = false;
                while (true)
                {
                    try
                    {
                        if (TcpClient.Available > 0)
                        {
                            byte[] buffer = new byte[TcpClient.Available];
                            var size = await NetworkStream.ReadAsync(buffer, 0, TcpClient.Available);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                MessageReceived?.Invoke(this, buffer);
                                TranspondCharacteristicWrapper.Value = buffer;
                                TranspondCharacteristicWrapper.NotifyAll();

                            });
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.Message);
                        error = true;
                        break;
                    }
                }
                if (error)
                {
                    State = TcpConnState.Error;
                    StateChanged?.Invoke(this, State);
                }
            });
        }

        public async Task SendAsync(byte[] message)
        {
            if(NetworkStream == null)
            {
                throw new NotImplementedException("NetworkStream is null");
            }
            try
            {
                await NetworkStream.WriteAsync(message, 0, message.Length);
            }
            catch(Exception exception)
            {
                Debug.WriteLine(exception);
                State = TcpConnState.Error;
                StateChanged?.Invoke(this, State);
            }
            
        }

        public void Restore()
        {
            if(State == TcpConnState.Error)
            {
                try
                {
                    NetworkStream.Close();

                }
                catch (Exception)
                {

                }
                try
                {
                    TcpClient.Close();
                    
                }
                catch (Exception)
                {

                }
                TcpClient = new TcpClient();
                NetworkStream = null;
                State = TcpConnState.Created;
                StateChanged?.Invoke(this, State);
            }
        }
        
    }

    public class TranspondCharacteristicWrapper
    {
        public ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper { get; private set; }
        public static Guid UUID = BluetoothUtils.ShortValueUuid(0x1234);
        private static GattCharacteristicProperties PROPERTIES = new GattCharacteristicProperties
        {
            Read = true,
            Write = true,
            Notify = true,
        };
        private static GattPermissions PERMISSIONS = new GattPermissions
        {
            Read = true,
            Write = true
        };
        public IGattServerCharacteristic GattServerCharacteristic { get; private set; }
        public TranspondCharacteristicWrapper(IBluetoothManager bluetoothManager)
        {
            ClientCharacteristicConfigurationDescriptorWrapper = new ClientCharacteristicConfigurationDescriptorWrapper(bluetoothManager);
            var builder = bluetoothManager.NewGattCharacteristicBuilder();
            GattServerCharacteristic = builder.SetUuid(UUID)
                .AddDescriptors(ClientCharacteristicConfigurationDescriptorWrapper.GattServerDescriptor)
                .SetPermissions(PERMISSIONS)
                .SetProperties(PROPERTIES)
                .Build();
        }

        public void NotifyAll()
        {
            var clientConfigurations = ClientCharacteristicConfigurationDescriptorWrapper.ClientConfigurations;
            foreach (var pair in clientConfigurations)
            {
                if (pair.Value.Notifications)
                {
                    GattServerCharacteristic.NotifyValueChanged(pair.Key, false);
                }
            }
        }

        public byte[] Value
        {
            get
            {
                return GattServerCharacteristic.Value;
            }
            set
            {
                GattServerCharacteristic.Value = value;
            }
        }


    }
}
