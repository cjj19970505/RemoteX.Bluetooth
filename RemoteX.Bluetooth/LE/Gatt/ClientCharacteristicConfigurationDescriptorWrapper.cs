using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public class ClientCharacteristicConfigurationDescriptorWrapper
    {
        private static Guid CLIENT_CHARACTERISTIC_CONFIGURATION_UUID = BluetoothUtils.ShortValueUuid(0x2902);
        private static GattPermissions PERMISSIONS = new GattPermissions
        {
            Read = true,
            Write = true,
        };

        private Dictionary<IBluetoothDevice, Configuration> _ClientConfigurations;

        public Dictionary<IBluetoothDevice, Configuration> ClientConfigurations
        {
            get
            {
                return _ClientConfigurations;
            }
        }
        public Configuration this[IBluetoothDevice bluetoothDevice]
        {
            get
            {
                if (bluetoothDevice == null)
                {
                    throw new Exception("BluetoothDevice is NULL");
                }
                if (!_ClientConfigurations.ContainsKey(bluetoothDevice))
                {
                    _ClientConfigurations.Add(bluetoothDevice, new Configuration { Notifications = false, Indications = false });
                }
                return _ClientConfigurations[bluetoothDevice];
            }
            private set
            {
                if (bluetoothDevice == null)
                {
                    throw new Exception("BluetoothDevice is NULL");
                }
                if (!_ClientConfigurations.ContainsKey(bluetoothDevice))
                {
                    _ClientConfigurations.Add(bluetoothDevice, new Configuration { Notifications = false, Indications = false });
                }
                _ClientConfigurations[bluetoothDevice] = value;
            }
        }
        
        public IGattServerDescriptor GattServerDescriptor { get; private set; }
        public ClientCharacteristicConfigurationDescriptorWrapper(IBluetoothManager bluetoothManager)
        {
            _ClientConfigurations = new Dictionary<IBluetoothDevice, Configuration>();
            var builder = bluetoothManager.NewGattDescriptorBuilder();
            builder.SetUuid(CLIENT_CHARACTERISTIC_CONFIGURATION_UUID).SetPermissions(PERMISSIONS);
            GattServerDescriptor = builder.Build();
            GattServerDescriptor.OnRead += _OnRead;
            GattServerDescriptor.OnWrite += _OnWrite;
        }

        private void _OnWrite(object sender, WriteRequest e)
        {
            var configuration = Configuration.FromBytes(e.Value);
            this[e.Device] = configuration;
            GattServerDescriptor.Characteristic.Service.Server.SendResponse(e.Device, e.RequestId, null);
        }

        private void _OnRead(object sender, DescriptorReadRequest e)
        {
            GattServerDescriptor.Characteristic.Service.Server.SendResponse(e.Device, e.RequestId, this[e.Device].Value);
        }

        public struct Configuration
        {
            public bool Notifications;
            public bool Indications;
            public byte[] Value
            {
                get
                {
                    int valueCode = 0;
                    if (Notifications)
                    {
                        valueCode |= 1;
                    }
                    if (Indications)
                    {
                        valueCode |= (1 << 1);
                    }
                    var valueCodeBytes = BitConverter.GetBytes(valueCode);
                    return new byte[] { valueCodeBytes[0], valueCodeBytes[1] };
                }
            }

            public static Configuration FromBytes(byte[] bytes)
            {
                Configuration configuration = new Configuration();
                configuration.Notifications = (bytes[0] & 0x01) != 0;
                configuration.Indications = (bytes[0] & 0x02) != 0;
                return configuration;
            }
        }
    }
}
