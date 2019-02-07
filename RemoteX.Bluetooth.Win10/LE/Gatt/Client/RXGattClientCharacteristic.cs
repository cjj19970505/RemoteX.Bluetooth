using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Client;
using RemoteX.Bluetooth.LE.Gatt.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Bluetooth.Win10.LE.Gatt.Client
{
    internal class RXGattClientCharacteristic : IGattClientCharacteristic
    {
        public IGattClientService Service => throw new NotImplementedException();

        public byte[] LatestValue => throw new NotImplementedException();

        public IGattServerDescriptor[] Descriptors => throw new NotImplementedException();

        public GattPermissions Permissions => throw new NotImplementedException();

        public Bluetooth.LE.Gatt.GattCharacteristicProperties CharacteristicProperties => throw new NotImplementedException();

        public int CharacteristicValueHandle => throw new NotImplementedException();

        public Guid Uuid
        {
            get
            {
                return Win10Characteristic.Uuid;
            }
        }

        public GattCharacteristic Win10Characteristic { get; }

        public ushort AttributeHandle
        {
            get
            {
                return Win10Characteristic.AttributeHandle;
            }
        }

        public event EventHandler<byte[]> OnNotified;

        public Task<ReadCharacteristicValueResult> ReadCharacteristicValueAsync()
        {
            throw new NotImplementedException();
        }

        public RXGattClientCharacteristic(GattCharacteristic win10Characteristic)
        {
            Win10Characteristic = win10Characteristic;
        }
    }
}
