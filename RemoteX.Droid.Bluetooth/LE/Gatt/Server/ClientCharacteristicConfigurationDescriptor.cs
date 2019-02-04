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

namespace RemoteX.Droid.Bluetooth.LE.Gatt.Server
{
    class ClientCharacteristicConfigurationDescriptor: GattServer.GattServerService.GattServerCharacteristic.GattServerDescriptor
    {
        private static Guid CLIENT_CHARACTERISTIC_CONFIGURATION_UUID = BluetoothUtils.ShortValueUuid(0x2902);
        private static GattPermissions PERMISSIONS = new GattPermissions
        {
            Read = true,
            Write = true,
            
        };
        public ClientCharacteristicConfigurationDescriptor():base(CLIENT_CHARACTERISTIC_CONFIGURATION_UUID, PERMISSIONS)
        {
            Notifications = true;
        }
        public bool Notifications { get; private set; }
        public bool Indications { get; private set; }

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
        public override void OnWriteRequest(BluetoothDevice device, int requestId, bool preparedWrite, bool responseNeeded, int offset, byte[] value)
        {
            base.OnWriteRequest(device, requestId, preparedWrite, responseNeeded, offset, value);
            //Log.Info("BLEAdver", "OnWriteRequest PreparedWrite:" + preparedWrite + " responseNeeded" + responseNeeded);
        }
        public override void OnReadRequest(BluetoothDevice device, int requestId, int offset)
        {
            base.OnReadRequest(device, requestId, offset);
            (Characteristic.Service.Server as GattServer).DroidGattServer.SendResponse(device, requestId, GattStatus.Success, offset, Value);
        }

    }
}