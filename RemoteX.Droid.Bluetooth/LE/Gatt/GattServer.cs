using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth;
using static RemoteX.Droid.BluetoothManager;

namespace RemoteX.Droid.Bluetooth.LE.Gatt
{
    public partial class GattServer : IGattServer
    {
        public ulong Address => throw new NotImplementedException();

        internal Android.Bluetooth.BluetoothGattServer DroidGattServer { get; private set; }
        internal BluetoothManager BluetoothManager { get; private set; }

        [Obsolete("Not Finished Yet")]
        public bool IsSupported
        {
            get
            {
                return true;
            }
        }

        private ServerCallback _ServerCallback;
        private AdvertiserCallback _AdvertiserCallback;

        /// <summary>
        /// 用来储存这个Server上的Service
        /// </summary>
        private List<GattServerService> _GattServices;

        public IGattServerService[] Services
        {
            get
            {
                return _GattServices.ToArray();
            }
        }

        public GattServerService[] GattServices
        {
            get
            {
                return _GattServices.ToArray();
            }
        }

        internal GattServer(BluetoothManager bluetoothManager)
        {
            _GattServices = new List<GattServerService>();
            _ServerCallback = new ServerCallback(this);
            _AdvertiserCallback = new AdvertiserCallback();
            BluetoothManager = bluetoothManager;
            DroidGattServer = BluetoothManager.DroidBluetoothManager.OpenGattServer(Application.Context, _ServerCallback);

            //AddService(new DeviceInfomationService());
            //AddService(new BatteryService());
        }

        public void StartAdvertising()
        {
            /*
            Guid testGuid = Guid.NewGuid();
            Android.Bluetooth.LE.AdvertiseSettings advertiseSettings = new Android.Bluetooth.LE.AdvertiseSettings.Builder()
                .SetTxPowerLevel(Android.Bluetooth.LE.AdvertiseTx.PowerHigh)
                .SetConnectable(true)
                .SetTimeout(0)
                .SetAdvertiseMode(Android.Bluetooth.LE.AdvertiseMode.LowLatency)
                .Build();
            Android.Bluetooth.LE.AdvertiseData advertiseData = new Android.Bluetooth.LE.AdvertiseData.Builder()
                .SetIncludeTxPowerLevel(false)
                .SetIncludeDeviceName(true)
                .AddServiceUuid(ParcelUuid.FromString(SERVICE_DEVICE_INFORMATION.ToString()))
                .Build();
            Android.Bluetooth.LE.AdvertiseData scanResult = new Android.Bluetooth.LE.AdvertiseData.Builder()
                .AddServiceUuid(ParcelUuid.FromString(SERVICE_DEVICE_INFORMATION.ToString()))
                .Build();
            var advertisier = BluetoothManager.BluetoothAdapter.BluetoothLeAdvertiser;

            AddService(_SetUpDeviceInformationService());
            advertisier.StartAdvertising(advertiseSettings, advertiseData, _AdvertiserCallback);
            Log.Info("BLEAdver", "Ithinkitworks");*/
            Android.Bluetooth.LE.AdvertiseSettings advertiseSettings = new Android.Bluetooth.LE.AdvertiseSettings.Builder()
                .SetTxPowerLevel(Android.Bluetooth.LE.AdvertiseTx.PowerHigh)
                .SetConnectable(true)
                .SetTimeout(0)
                .SetAdvertiseMode(Android.Bluetooth.LE.AdvertiseMode.LowLatency)
                .Build();

            var advertiseDataBuilder = new Android.Bluetooth.LE.AdvertiseData.Builder().SetIncludeTxPowerLevel(false).SetIncludeDeviceName(true);
            var scanResultBuilder = new Android.Bluetooth.LE.AdvertiseData.Builder();

            foreach(var service in _GattServices)
            {
                Log.Info("BLEAdver", "Service:"+ service.Uuid.ToString());
                foreach(var chara in service.DroidService.Characteristics)
                {
                    Log.Info("BLEAdver", "Char:" + chara.Uuid.ToString());
                    foreach(var des in chara.Descriptors)
                    {
                        Log.Info("BLEAdver", "Des:" + des.Uuid);
                    }
                }
                advertiseDataBuilder.AddServiceUuid(service.Uuid.ToJavaParcelUuid());
                scanResultBuilder.AddServiceUuid(service.Uuid.ToJavaParcelUuid());
            }
            var advertiseData = advertiseDataBuilder.Build();
            var scanResult = scanResultBuilder.Build();
            var advertisier = BluetoothManager.BluetoothAdapter.BluetoothLeAdvertiser;
            advertisier.StartAdvertising(advertiseSettings, advertiseData, _AdvertiserCallback);
        }


        public void AddService(IGattServerService service)
        {
            (service as GattServerService).AddToServer(this);
        }
        BluetoothDevice _ConnectedDevice;
        public void NotifyTest()
        {
            try
            {
                DroidGattServer.NotifyCharacteristicChanged(_ConnectedDevice, _GattServices.GetFromUuid(BatteryService.BATTERY_SERVICE_UUID).GattCharacteristics.GetFromUuid(BatteryService.BatteryLevelCharacteristic.BATTERY_LEVEL_UUID).DroidCharacteristic, false);
            }
            catch(Exception e)
            {

            }
        }
        public void SendResponse(IBluetoothDevice bluetoothDevice, int requestId, byte[] responseBytes)
        {
            //GattServer.DroidGattServer.SendResponse(device, requestId, GattStatus.Success, offset, Encoding.Default.GetBytes("XJSayHello"+requestId));
            DroidGattServer.SendResponse((bluetoothDevice as BluetoothDeviceWrapper).DroidDevice, requestId, GattStatus.Success, 0, responseBytes);
        }

        private class ServerCallback : Android.Bluetooth.BluetoothGattServerCallback
        {
            public GattServer GattServer { get; private set; }
            public ServerCallback(GattServer gattServer)
            {
                GattServer = gattServer;
            }
            public override void OnCharacteristicReadRequest(BluetoothDevice device, int requestId, int offset, BluetoothGattCharacteristic droidCharacteristic)
            {
                
                base.OnCharacteristicReadRequest(device, requestId, offset, droidCharacteristic);
                var service = GattServer._GattServices.GetFromUuid(droidCharacteristic.Service.Uuid.ToGuid());
                var characteristic = service.GattCharacteristics.GetFromUuid(droidCharacteristic.Uuid.ToGuid());
                characteristic.OnCharacteristicRead(device, requestId, offset);
                //GattServer.DroidGattServer.SendResponse(device, requestId, GattStatus.Success, offset, Encoding.Default.GetBytes("XJSayHello"+requestId));
                //characteristic.SetValue(Encoding.Default.GetBytes("FUCK" + requestId));

                //GattServer._ConnectedDevice = device;
                //Log.Info("BLEAdver", "OnCharacteristicReadRequest " + characteristic.Uuid+" Handle:"+characteristic.Handle);
            }
            public override void OnCharacteristicWriteRequest(BluetoothDevice device, int requestId, BluetoothGattCharacteristic droidCharacteristic, bool preparedWrite, bool responseNeeded, int offset, byte[] value)
            {
                base.OnCharacteristicWriteRequest(device, requestId, droidCharacteristic, preparedWrite, responseNeeded, offset, value);

                Log.Info("BLEAdver", "OnCharacteristicWriteRequest Device:"+device.ToString() );
                var service = GattServer._GattServices.GetFromUuid(droidCharacteristic.Service.Uuid.ToGuid());
                var characteristic = service.GattCharacteristics.GetFromUuid(droidCharacteristic.Uuid.ToGuid());
                characteristic.OnCharacteristicWrite(device, requestId, droidCharacteristic, preparedWrite, responseNeeded, offset, value);

                //GattServer.DroidGattServer.SendResponse(device, requestId, GattStatus.Success, offset, value);

            }

            public override void OnDescriptorWriteRequest(BluetoothDevice device, int requestId, BluetoothGattDescriptor droidDescriptor, bool preparedWrite, bool responseNeeded, int offset, byte[] value)
            {
                GattServer._ConnectedDevice = device;
                base.OnDescriptorWriteRequest(device, requestId, droidDescriptor, preparedWrite, responseNeeded, offset, value);
                var descriptor = droidDescriptor.ToDescriptor(GattServer);
                GattServer.DroidGattServer.SendResponse(device, requestId, GattStatus.Success, offset, value);
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                for (int i = 0; i < value.Length; i++)
                {
                    sb.Append(value[i]);
                    if(i != value.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append(")");
                Log.Info("BLEAdver", "OnDescriptorWriteRequest: "+sb.ToString());
                descriptor.OnWriteRequest(device, requestId, preparedWrite, responseNeeded, offset, value);
                
            }
            public override void OnDescriptorReadRequest(BluetoothDevice device, int requestId, int offset, BluetoothGattDescriptor droidDescriptor)
            {
                base.OnDescriptorReadRequest(device, requestId, offset, droidDescriptor);
                var descriptor = droidDescriptor.ToDescriptor(GattServer);
                descriptor.OnReadRequest(device, requestId, offset);
                Log.Info("BLEAdver", "OnDescriptorReadRequest");
            }
            public override void OnExecuteWrite(BluetoothDevice device, int requestId, bool execute)
            {
                base.OnExecuteWrite(device, requestId, execute);
                Log.Info("BLEAdver", "OnExecuteWrite");
            }
            public override void OnConnectionStateChange(BluetoothDevice device, [GeneratedEnum] ProfileState status, [GeneratedEnum] ProfileState newState)
            {
                base.OnConnectionStateChange(device, status, newState);
                Log.Info("BLEAdver", "OnExecuteWrite State:"+status+" New"+ newState);

            }


        }
    

        private class AdvertiserCallback : Android.Bluetooth.LE.AdvertiseCallback
        {
            public override void OnStartSuccess(AdvertiseSettings settingsInEffect)
            {
                base.OnStartSuccess(settingsInEffect);
            }
            public override void OnStartFailure([GeneratedEnum] AdvertiseFailure errorCode)
            {
                base.OnStartFailure(errorCode);
            }
        }

        /*
        private static Guid SERVICE_DEVICE_INFORMATION = BluetoothUtils.ShortValueUuid(0x180A);
        private static Guid CHARACTERISTIC_MANUFACTURER_NAME = BluetoothUtils.ShortValueUuid(0x2A29);
        private static Guid CHARACTERISTIC_MODEL_NUMBER = BluetoothUtils.ShortValueUuid(0x2A24);
        private static Guid CHARACTERISTIC_SERIAL_NUMBER = BluetoothUtils.ShortValueUuid(0x2A25);

        
        private static GattServerService _SetUpDeviceInformationService()
        {
            GattServerService gattService = new GattServerService(SERVICE_DEVICE_INFORMATION);
            GattCharacteristicProperties properties = new GattCharacteristicProperties
            {
                Read = true,
                WriteWithoutResponse = true,
                Write = true,
                Notify = true

            };
            GattPermissions gattPermissions = new GattPermissions
            {
                Read = true,
                Write = true
            };
            gattService.AddCharacteristic(new GattServerCharacteristic(CHARACTERISTIC_MANUFACTURER_NAME, properties, gattPermissions));
            gattService.AddCharacteristic(new GattServerCharacteristic(CHARACTERISTIC_MODEL_NUMBER, properties, gattPermissions));
            gattService.AddCharacteristic(new GattServerCharacteristic(CHARACTERISTIC_SERIAL_NUMBER, properties, gattPermissions));
            return gattService;
        }
        */
    }
}