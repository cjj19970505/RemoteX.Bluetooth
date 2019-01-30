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

        private void _OnWrite(object sender, IDescriptorWriteRequest e)
        {
            var configuration = Configuration.FromBytes(e.Value);
            if(this[e.SourceDevice] != configuration)
            {
                this[e.SourceDevice] = configuration;
            }
                
            System.Diagnostics.Debug.WriteLine("XJ:::"+ Configuration.FromBytes(configuration.Value));
            e.RespondSuccess();
        }

        private void _OnRead(object sender, IDescriptorReadRequest e)
        {
            e.RespondWithValue(this[e.SourceDevice].Value);
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

            public override string ToString()
            {
                return "Notification:" + Notifications + " Indication:" + Indications;
            }

            public static bool operator ==(Configuration lhs, Configuration rhs)
            {
                if(lhs.Indications == rhs.Indications && lhs.Notifications == rhs.Notifications)
                {
                    return true;
                }
                return false;
            }

            public static bool operator !=(Configuration lhs, Configuration rhs)
            {
                return !(lhs == rhs);
            }
        }
    }
}
