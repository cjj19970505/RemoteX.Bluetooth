using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt;

namespace RemoteX.Droid.Bluetooth.LE.Gatt
{
    internal class DeviceInfomationService:GattServer.GattServerService
    {
        private static Guid SERVICE_DEVICE_INFORMATION = BluetoothUtils.ShortValueUuid(0x180A);
        private static Guid CHARACTERISTIC_MANUFACTURER_NAME = BluetoothUtils.ShortValueUuid(0x2A29);
        private static Guid CHARACTERISTIC_MODEL_NUMBER = BluetoothUtils.ShortValueUuid(0x2A24);
        private static Guid CHARACTERISTIC_SERIAL_NUMBER = BluetoothUtils.ShortValueUuid(0x2A25);

        public DeviceInfomationService() : base(SERVICE_DEVICE_INFORMATION)
        {
            AddCharacteristic(new ManufacturerNameStringCharacteristic("XJStudio"));
        }

        class ManufacturerNameStringCharacteristic : GattServerCharacteristic
        {
            private static Guid CHARACTERISTIC_MANUFACTURER_NAME = BluetoothUtils.ShortValueUuid(0x2A29);
            private static GattPermissions PERMISSIONS = new GattPermissions
            {
                Read = true
            };
            private static GattCharacteristicProperties PROPERTIES = new GattCharacteristicProperties
            {
                Read = true
            };
            public string ManufacturerName { get; private set; }
            public ManufacturerNameStringCharacteristic(string manufacturerName) : base(CHARACTERISTIC_MANUFACTURER_NAME, PROPERTIES, PERMISSIONS)
            {
                ManufacturerName = manufacturerName;
            }
            public override void OnCharacteristicRead(BluetoothDevice device, int requestId, int offset)
            {
                base.OnCharacteristicRead(device, requestId, offset);
                (Service.Server as GattServer).DroidGattServer.SendResponse(device, requestId, GattStatus.Success, offset, Encoding.Default.GetBytes(ManufacturerName));
                Log.Info("BLEAdver", "OnCharacteristicReadRequest " + " Handle:" + this.CharacteristicValueHandle);
            }

        }
    }
}