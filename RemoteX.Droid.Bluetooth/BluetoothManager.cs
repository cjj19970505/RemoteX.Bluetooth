using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;
using RemoteX.Bluetooth.Droid.LE.Gatt;
using RemoteX.Bluetooth.Droid.LE.Gatt.Server;
using RemoteX.Bluetooth.Rfcomm;
using System.Threading.Tasks;
using RemoteX.Bluetooth.Droid.Rfcomm;

//[assembly: Xamarin.Forms.Dependency(typeof(RemoteX.Droid.BluetoothManager))]
namespace RemoteX.Bluetooth.Droid
{
    /// <summary>
    /// 对IBluetoothManager的Android端的实现
    /// 使用使和Android.Bluetooth.BluetoothManager区分开来，两个不一样
    /// 
    /// </summary>
    public partial class BluetoothManager : RemoteX.Bluetooth.IBluetoothManager
    {
        public BluetoothAdapter BluetoothAdapter { get; private set;}
        bool _IsDiscoverying;

        public Android.Bluetooth.BluetoothManager DroidBluetoothManager { get; private set; }

        public List<BluetoothDeviceWrapper> _KnownBluetoothDevices;

        public BluetoothManager()
        {
            _IsDiscoverying = false;
            BluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            RfcommScanner = new RfcommScanner(this);
            _KnownBluetoothDevices = new List<BluetoothDeviceWrapper>();
            DroidBluetoothManager = Application.Context.GetSystemService(Context.BluetoothService) as Android.Bluetooth.BluetoothManager;
            ServiceProviderList = new List<IRfcommServiceProvider>();

        }
        public bool IsDiscoverying
        {
            get
            {
                return _IsDiscoverying;
            }
            private set
            {
                this._IsDiscoverying = value;
            }
        }
        public bool Supported
        {
            get
            {
                if (BluetoothAdapter != null)
                {
                    return true;
                }
                return false;
            }
        }
        public string Name
        {
            get
            {
                return BluetoothAdapter.Name;
            }
        }

        /// <summary>
        /// According to https://developer.android.com/about/versions/marshmallow/android-6.0-changes.html#behavior-hardware-id this only returns 02:00:00:00:00:00
        /// </summary>
        public ulong MacAddress
        {
            get
            {
                //android.provider.Settings.Secure.getString(context.getContentResolver(), "bluetooth_address");
                //var strAddr = Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, "bluetooth_address");
                var strAddr = BluetoothAdapter.Address;
                
                //return BluetoothUtils.AddressStringToInt64(BluetoothAdapter.Address);
                return BluetoothUtils.AddressStringToInt64(strAddr);
            }
        }

        public IBluetoothLEScanner LEScanner => throw new NotImplementedException();

        public IBluetoothRfcommScanner RfcommScanner { get; }

        private List<IRfcommServiceProvider> ServiceProviderList;
        public IRfcommServiceProvider[] ServiceProviders
        {
            get
            {
                return ServiceProviderList.ToArray();
            }
        }

        public IBluetoothDevice GetBluetoothDevice(ulong macAddress)
        {
            byte[] addressBytesULong = BitConverter.GetBytes(macAddress);
            byte[] addressBytes = new byte[6];
            for(int i =0;i<addressBytes.Length;i++)
            {
                addressBytes[i] = addressBytesULong[5-i];
            }
            BluetoothDevice device = BluetoothAdapter.GetRemoteDevice(addressBytes);
            return BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice(this, device);
        }

        public IGattServiceBuilder NewGattServiceBuilder()
        {
            return new GattServiceBuilder();
        }

        public IGattCharacteristicBuilder NewGattCharacteristicBuilder()
        {
            return new GattCharacteristicBuilder();
        }

        public IGattDescriptorBuilder NewGattDescriptorBuilder()
        {
            return new GattDescriptorBuilder();
        }

        public Task<IRfcommServiceProvider> CreateRfcommServiceProviderAsync(Guid serviceId)
        {
            return Task.Run(() =>
            {
                var serviceProvider = new RfcommServiceProvider(this, serviceId);
                ServiceProviderList.Add(serviceProvider);
                return serviceProvider as IRfcommServiceProvider;
            });
        }

        public RemoteX.Bluetooth.IBluetoothDevice[] PairedDevices
        {
            get
            {
                ICollection<BluetoothDevice> pairedDevices = BluetoothAdapter.BondedDevices;
                List<RemoteX.Bluetooth.IBluetoothDevice> devices = new List<RemoteX.Bluetooth.IBluetoothDevice>();
                foreach(var droidDevice in pairedDevices)
                {
                    BluetoothDeviceWrapper bluetoothDeviceWrapper = BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice(this, droidDevice);
                    devices.Add(bluetoothDeviceWrapper);
                }
                return devices.ToArray();
            }
        }

        private GattServer _GattServer;
        public IGattServer GattSever
        {
            get
            {
                if(_GattServer == null)
                {
                    _GattServer = new GattServer(this);
                }
                return _GattServer;
            }
        }

        class ConnectionStateChangeReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                throw new NotImplementedException();
            }
        }


    }
}