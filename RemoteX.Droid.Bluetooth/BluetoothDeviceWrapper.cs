using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using RemoteX.Bluetooth;
using RemoteX.Bluetooth.Droid.Rfcomm;
using RemoteX.Bluetooth.LE.Gatt.Client;
using RemoteX.Bluetooth.Rfcomm;

namespace RemoteX.Bluetooth.Droid
{
    public partial class BluetoothManager
    {
        /// <summary>
        /// 针对Xamarin.Form中IBluetoothDevice对Android的BluetoothDevice进行的一层包装
        /// 目前不足：在FetchUuid的时候要是遇到蓝牙或者Discovery被中途退出，或者一直获取不到uuid，IsFetchingUuids就会一直为true
        /// </summary>
        public class BluetoothDeviceWrapper : RemoteX.Bluetooth.IBluetoothDevice
        {
            Receiver _Receiver;

            public string Name
            {
                get
                {
                    return DroidDevice.Name;
                }
            }

            public ulong Address
            {
                get
                {
                    return BluetoothUtils.AddressStringToInt64(DroidDevice.Address);
                }
            }

            public List<RfcommDeviceService> _Services;

            Thread FetchingUuidThread;

            public Android.Bluetooth.BluetoothDevice DroidDevice { get; private set; }

            public IGattClient GattClient => throw new NotImplementedException();

            [Obsolete("Will this update the BluetoothDevice locally ?")]
            /// <summary>
            /// Will this update the BluetoothDevice locally ?
            /// Not yet! But I think it will be someday
            /// </summary>
            /// <param name="bluetoothManager"></param>
            /// <param name="droidDevice"></param>
            /// <returns></returns>
            public static BluetoothDeviceWrapper GetBluetoothDeviceFromDroidDevice(BluetoothManager bluetoothManager, Android.Bluetooth.BluetoothDevice droidDevice)
            {
                var existDevice = bluetoothManager._KnownBluetoothDevices.GetFromAddress(droidDevice.Address);
                if(existDevice == null)
                {
                    existDevice = new BluetoothDeviceWrapper(droidDevice);
                    bluetoothManager._KnownBluetoothDevices.Add(existDevice);
                }
                return existDevice;
            }

            public RfcommDeviceService GetRXServiceFromDroidService(Guid droidService)
            {
                RfcommDeviceService rxService = null;
                foreach (var service in _Services)
                {
                    if (service.ServiceId == droidService)
                    {
                        rxService = service as RfcommDeviceService;
                    }
                }
                if (rxService == null)
                {
                    rxService = new RfcommDeviceService(this, droidService);
                    _Services.Add(rxService);
                }
                return rxService;
            }

            private BluetoothDeviceWrapper(Android.Bluetooth.BluetoothDevice bluetoothDevice)
            {
                DroidDevice = bluetoothDevice;
                _Receiver = new Receiver(this);
                _Services = new List<RfcommDeviceService>();
            }

            public override string ToString()
            {
                return "(DEVICE:: Name: " + Name + ", Mac: " + Address + ")";
            }

            public Task RfcommConnectAsync()
            {
                return Task.Run(() =>
                {

                });
            }

            public async Task<RfcommDeviceServiceResult> GetRfcommServicesAsync()
            {
                //Do some test here;
                {
                    Guid guid = Guid.NewGuid();
                    Java.Nio.ByteBuffer bb = Java.Nio.ByteBuffer.Wrap(guid.ToByteArray());
                    long firstLong = bb.Long;
                    long secondLong = bb.Long;
                    UUID uuid = new UUID(firstLong, secondLong);
                    System.Diagnostics.Debug.WriteLine("1 GUID:" + guid.ToString());
                    System.Diagnostics.Debug.WriteLine("1 UUID:" + uuid.ToString());

                    guid = Guid.Parse(uuid.ToString());
                    System.Diagnostics.Debug.WriteLine("2 GUID:" + guid.ToString());
                    System.Diagnostics.Debug.WriteLine("2 UUID:" + uuid.ToString());
                }
                IntentFilter intentFilter = new IntentFilter(Android.Bluetooth.BluetoothDevice.ActionUuid);
                Application.Context.RegisterReceiver(_Receiver, intentFilter);
                
                return await Task.Run(() =>
                {
                    FetchingUuidThread = Thread.CurrentThread;
                    DroidDevice.FetchUuidsWithSdp();
                    try
                    {
                        Thread.Sleep(Timeout.Infinite);
                        FetchingUuidThread = null;
                    }
                    catch(ThreadInterruptedException)
                    {
                        FetchingUuidThread = null;
                    }
                    return _Receiver.RfcommDeviceServiceResult;

                });
            }

            public Task<RfcommDeviceServiceResult> GetRfcommServicesForIdAsync(Guid serviceId)
            {
                throw new NotImplementedException();
            }

            private class Receiver : BroadcastReceiver
            {
                private BluetoothDeviceWrapper _DeviceWrapper;
                public RfcommDeviceServiceResult RfcommDeviceServiceResult { get; private set; }
                public Receiver(BluetoothDeviceWrapper deviceWrapper)
                {
                    _DeviceWrapper = deviceWrapper;
                }

                public override void OnReceive(Context context, Intent intent)
                {
                    string action = intent.Action;
                    if (Android.Bluetooth.BluetoothDevice.ActionUuid == action)
                    {
                        IParcelable[] parcelUuids = intent.GetParcelableArrayExtra(Android.Bluetooth.BluetoothDevice.ExtraUuid);
                        List<Guid> guids = new List<Guid>();
                        if (parcelUuids != null && parcelUuids.Length > 0)
                        {
                            foreach (IParcelable parcel in parcelUuids)
                            {
                                ParcelUuid parcelUuid = parcel as ParcelUuid;
                                if (parcelUuid != null)
                                {
                                    guids.Add(parcelUuid.Uuid.ToGuid());
                                }
                            }
                        }
                        Application.Context.UnregisterReceiver(this);
                        List<IRfcommDeviceService> serviceList = new List<IRfcommDeviceService>();
                        foreach(var guid in guids)
                        {
                            var service = _DeviceWrapper.GetRXServiceFromDroidService(guid);
                            serviceList.Add(service);
                        }
                        RfcommDeviceServiceResult = new RfcommDeviceServiceResult()
                        {
                            Error = BluetoothError.Success,
                            Services = serviceList.ToArray()
                        };
                        if(_DeviceWrapper.FetchingUuidThread != null)
                        {
                            _DeviceWrapper.FetchingUuidThread.Interrupt();
                        }
                    }
                }
            }

        }
    }
    
}