using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public class KeepNotifyingCharacteristicWrapper
    {
        public IGattCharacteristicBuilder Builder;
        public static Guid UUID = BluetoothUtils.ShortValueUuid(0x2A11);
        private static GattCharacteristicProperties PROPERTIES = new GattCharacteristicProperties
        {
            Read = true,
            Notify = true,
        };
        private static GattPermissions PERMISSIONS = new GattPermissions
        {
            Read = true,
        };

        private long _NotifyCount;

        public ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper { get; }
        public IGattServerCharacteristic GattServerCharacteristic { get; }

        private TimeSpan _Interval;
        public TimeSpan Interval
        {
            get
            {
                return _Interval;
            }
            set
            {
                _Interval = value;
            }
        }

        public bool Notifying { get; private set; }

        public int NotifyLength { get; set; }

        public KeepNotifyingCharacteristicWrapper(IBluetoothManager bluetoothManager)
        {
            ClientCharacteristicConfigurationDescriptorWrapper = new ClientCharacteristicConfigurationDescriptorWrapper(bluetoothManager);
            IGattCharacteristicBuilder builder = bluetoothManager.NewGattCharacteristicBuilder();
            builder.SetUuid(UUID).SetPermissions(PERMISSIONS).SetProperties(PROPERTIES);
            builder.AddDescriptors(ClientCharacteristicConfigurationDescriptorWrapper.GattServerDescriptor);
            GattServerCharacteristic = builder.Build();
            GattServerCharacteristic.OnRead += _OnRead;

        }

        private void _OnRead(object sender, ICharacteristicReadRequest e)
        {
            e.RespondWithValue(null);
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
        
        public Task KeepNotifyAsync()
        {
            Notifying = true;
            return Task.Run(() =>
            {
                DateTime latestNotifyDateTime = DateTime.Now;
                while (Notifying)
                {
                    TimeSpan deltaTime = DateTime.Now - latestNotifyDateTime;
                    
                    if (deltaTime < Interval)
                    {
                        continue;
                    }
                    latestNotifyDateTime = DateTime.Now;
                    //GattServerCharacteristic.Value = BitConverter.GetBytes((int)deltaTime.TotalMilliseconds);
                    byte[] notifyBytes = new byte[NotifyLength];
                    byte[] notifyCountBytes = BitConverter.GetBytes(_NotifyCount);
                    for(int i = 0; i < notifyCountBytes.Length; i++)
                    {
                        notifyBytes[i] = notifyCountBytes[i];
                    }
                    GattServerCharacteristic.Value = notifyBytes;
                    NotifyAll();
                    System.Diagnostics.Debug.WriteLine("_NotifyCount:" + _NotifyCount);
                    _NotifyCount++;
                }
            });
        }

        public void StopNotify()
        {
            _NotifyCount = 0;
            Notifying = false;
        }
    }
}
