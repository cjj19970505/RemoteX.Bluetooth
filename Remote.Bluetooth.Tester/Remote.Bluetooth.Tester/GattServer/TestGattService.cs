using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Remote.Bluetooth.Tester.GattServer
{
    class TestGattService
    {
        public IGattServerService GattServerService { get; }
        public TestCharacteristicWrapper TestCharacteristicWrapper { get; }
        public TestGattService(IBluetoothManager bluetoothManager, Int32 shortUuid)
        {
            TestCharacteristicWrapper = new TestCharacteristicWrapper(bluetoothManager);
            
            IGattServiceBuilder builder = bluetoothManager.NewGattServiceBuilder();
            GattServerService = builder.SetUuid(BluetoothUtils.ShortValueUuid(shortUuid))
                .AddCharacteristics(TestCharacteristicWrapper.GattCharacteristic)
                .Build();

            TestCharacteristicWrapper.GattCharacteristic.OnWrite += _OnCharacteristicWrite;
            TestCharacteristicWrapper.GattCharacteristic.OnRead += _OnCharacteristicRead;
        }

        private void _OnCharacteristicRead(object sender, CharacteristicReadRequest e)
        {
            throw new NotImplementedException();
        }

        private void _OnCharacteristicWrite(object sender, WriteRequest e)
        {
            throw new NotImplementedException();
        }
    }

    class TestCharacteristicWrapper
    {
        public ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper { get; private set; }
        public static Guid UUID = BluetoothUtils.ShortValueUuid(0x7823);
        private static GattCharacteristicProperties PROPERTIES = new GattCharacteristicProperties
        {
            Read = true,
            Notify = true,
        };
        private static GattPermissions PERMISSIONS = new GattPermissions
        {
            Read = true,
        };
        public IGattServerCharacteristic GattCharacteristic { get; }
        public TestCharacteristicWrapper(IBluetoothManager bluetoothManager)
        {
            ClientCharacteristicConfigurationDescriptorWrapper = new ClientCharacteristicConfigurationDescriptorWrapper(bluetoothManager);
            var builder = bluetoothManager.NewGattCharacteristicBuilder();
            GattCharacteristic = builder.SetUuid(UUID)
                .AddDescriptors(ClientCharacteristicConfigurationDescriptorWrapper.GattServerDescriptor)
                .SetPermissions(PERMISSIONS)
                .SetProperties(PROPERTIES)
                .Build();

        }
    }
}
