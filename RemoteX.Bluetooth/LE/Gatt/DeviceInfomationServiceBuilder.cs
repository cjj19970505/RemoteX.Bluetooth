using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public class DeviceInfomationServiceBuilder
    {
        //private static Guid SERVICE_DEVICE_INFORMATION = BluetoothUtils.ShortValueUuid(0x180A);
        private static Guid SERVICE_DEVICE_INFORMATION = BluetoothUtils.ShortValueUuid(0x180A);
        private static Guid CHARACTERISTIC_MANUFACTURER_NAME = BluetoothUtils.ShortValueUuid(0x2A29);
        private static Guid CHARACTERISTIC_MODEL_NUMBER = BluetoothUtils.ShortValueUuid(0x2A24);
        private static Guid CHARACTERISTIC_SERIAL_NUMBER = BluetoothUtils.ShortValueUuid(0x2A25);

        private IGattServiceBuilder _ServiceBuilder;
        public IBluetoothManager BluetoothManager { get; private set; }

        public DeviceInfomationServiceBuilder(IBluetoothManager bluetoothManager)
        {
            BluetoothManager = bluetoothManager;
            var manufacturerNameStringCharacteristic = new ManufacturerNameStringCharacteristicBuilder(BluetoothManager).Build();
            manufacturerNameStringCharacteristic.OnRead += OnManufacturerNameStringCharacteristicRead;
            manufacturerNameStringCharacteristic.OnWrite += OnManufacturerNameStringCharacteristicWrite;
            _ServiceBuilder = BluetoothManager.NewGattServiceBuilder().SetUuid(SERVICE_DEVICE_INFORMATION)
                                .SetServiceType(GattServiceType.Primary)
                                .AddCharacteristics(manufacturerNameStringCharacteristic);
        }

        private void OnManufacturerNameStringCharacteristicWrite(object sender, WriteRequest e)
        {
            (sender as IGattServerCharacteristic).Service.Server.SendResponse(e.Device, e.RequestId, null);
        }

        private void OnManufacturerNameStringCharacteristicRead(object sender, CharacteristicReadRequest e)
        {
            (sender as IGattServerCharacteristic).Service.Server.SendResponse(e.Device, e.RequestId, null);
        }

        public IGattServerService Build()
        {
            return _ServiceBuilder.Build();
        }

        public class ManufacturerNameStringCharacteristicBuilder
        {
            private static Guid CHARACTERISTIC_MANUFACTURER_NAME = BluetoothUtils.ShortValueUuid(0x2A29);
            private static GattPermissions PERMISSIONS = new GattPermissions
            {
                Read = true,
            };
            private static GattCharacteristicProperties PROPERTIES = new GattCharacteristicProperties
            {
                Read = true,
            };
            private IGattCharacteristicBuilder _Builder;

            public ManufacturerNameStringCharacteristicBuilder(IBluetoothManager bluetoothManager)
            {
                _Builder = bluetoothManager.NewGattCharacteristicBuilder();
                _Builder.SetUuid(CHARACTERISTIC_MANUFACTURER_NAME)
                    .SetPermissions(PERMISSIONS)
                    .SetProperties(PROPERTIES);
            }
            public IGattServerCharacteristic Build()
            {
                return _Builder.Build();
            }
        }
    }
}
