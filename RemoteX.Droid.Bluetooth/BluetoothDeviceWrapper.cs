using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using RemoteX.Bluetooth;

namespace RemoteX.Droid
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

            public Guid[] LastestFetchedUuids { get; private set; }

            public bool IsFetchingUuids { get; private set; }

            public event RemoteX.Bluetooth.BluetoothDeviceGetUuidsHanlder OnUuidsFetched;

            public Android.Bluetooth.BluetoothDevice DroidDevice { get; private set; }

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

            private BluetoothDeviceWrapper(Android.Bluetooth.BluetoothDevice bluetoothDevice)
            {
                this.DroidDevice = bluetoothDevice;
                IsFetchingUuids = false;
                _Receiver = new Receiver(this);
                ParcelUuid[] uuids = bluetoothDevice.GetUuids();
                if (uuids != null)
                {
                    Guid[] guids = new Guid[uuids.Length];
                    for (int i = 0; i < uuids.Length; i++)
                    {
                        Guid guid = Guid.Parse(uuids[i].Uuid.ToString());
                        guids[i] = guid;
                    }
                    LastestFetchedUuids = guids;
                }
            }

            public void FetchUuidsWithSdp()
            {
                if (IsFetchingUuids)
                {
                    return;
                }
                IsFetchingUuids = true;
                IntentFilter intentFilter = new IntentFilter(Android.Bluetooth.BluetoothDevice.ActionUuid);
                Application.Context.RegisterReceiver(_Receiver, intentFilter);
                DroidDevice.FetchUuidsWithSdp();
            }

            public void stopFetchingUuidsWithSdp()
            {
                if (!IsFetchingUuids)
                {
                    return;
                }
                IsFetchingUuids = false;
                Android.Bluetooth.BluetoothAdapter.DefaultAdapter.CancelDiscovery();
                Application.Context.UnregisterReceiver(_Receiver);
            }


            private class Receiver : BroadcastReceiver
            {
                private BluetoothDeviceWrapper _DeviceWrapper;
                public Receiver(BluetoothDeviceWrapper deviceWrapper)
                {
                    this._DeviceWrapper = deviceWrapper;
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
                                    guids.Add(Guid.Parse(parcelUuid.Uuid.ToString()));
                                }
                            }
                        }
                        Application.Context.UnregisterReceiver(this);
                        _DeviceWrapper.IsFetchingUuids = false;
                        _DeviceWrapper.LastestFetchedUuids = guids.ToArray();
                        _DeviceWrapper.OnUuidsFetched?.Invoke(_DeviceWrapper, guids.ToArray());
                    }
                }
            }

        }
    }
    
}