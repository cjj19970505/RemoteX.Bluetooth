using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Remote.Bluetooth.Tester.GattServer
{
    public class TestGattServiceWrapper
    {
        public IGattServerService GattServerService { get; }
        public TestCharacteristicWrapper TestCharacteristicWrapper { get; }
        public IBluetoothManager BluetoothManager { get; }
        public ObservableCollection<GattRequestViewModel> GattRequestViewModels;
        byte[] value;
        public TestGattServiceWrapper(IBluetoothManager bluetoothManager, Int32 shortUuid)
        {
            GattRequestViewModels = new ObservableCollection<GattRequestViewModel>();
            BluetoothManager = bluetoothManager;
            TestCharacteristicWrapper = new TestCharacteristicWrapper(bluetoothManager);
            
            IGattServiceBuilder builder = bluetoothManager.NewGattServiceBuilder();
            GattServerService = builder.SetUuid(BluetoothUtils.ShortValueUuid(shortUuid))
                .AddCharacteristics(TestCharacteristicWrapper.GattServerCharacteristic)
                .Build();

            TestCharacteristicWrapper.GattServerCharacteristic.OnWrite += _OnCharacteristicWrite;
            TestCharacteristicWrapper.GattServerCharacteristic.OnRead += _OnCharacteristicRead;
            value = BitConverter.GetBytes(232);
        }

        private void _OnCharacteristicWrite(object sender, ICharacteristicWriteRequest e)
        {
            GattRequestViewModels.Add(new GattRequestViewModel(e));
            value = e.Value;
        }

        private void _OnCharacteristicRead(object sender, ICharacteristicReadRequest e)
        {
            GattRequestViewModels.Add(new GattRequestViewModel(e));
        }
    }

    public class TestCharacteristicWrapper
    {
        public ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper { get; private set; }
        public static Guid UUID = BluetoothUtils.ShortValueUuid(0x7823);
        private static GattCharacteristicProperties PROPERTIES = new GattCharacteristicProperties
        {
            Read = true,
            Write= true,
            Notify = true,
        };
        private static GattPermissions PERMISSIONS = new GattPermissions
        {
            Read = true,
            Write = true
        };
        public IGattServerCharacteristic GattServerCharacteristic { get; }
        public TestCharacteristicWrapper(IBluetoothManager bluetoothManager)
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
    }
}
