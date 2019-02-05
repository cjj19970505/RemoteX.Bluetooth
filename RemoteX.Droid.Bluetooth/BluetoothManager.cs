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
using RemoteX.Droid.Bluetooth.LE.Gatt;
using RemoteX.Droid.Bluetooth.LE.Gatt.Server;

//[assembly: Xamarin.Forms.Dependency(typeof(RemoteX.Droid.BluetoothManager))]
namespace RemoteX.Droid
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
        Receiver _DiscoveryStartedReceiver;
        Receiver _DevicesFoundReceiver;
        Receiver _DiscoveryFinishedReceiver;

        public Android.Bluetooth.BluetoothManager DroidBluetoothManager { get; private set; }

        public List<BluetoothDeviceWrapper> _KnownBluetoothDevices;

        public BluetoothManager()
        {
            _IsDiscoverying = false;
            BluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            _DiscoveryStartedReceiver = new Receiver(this);
            _DevicesFoundReceiver = new Receiver(this);
            _DiscoveryFinishedReceiver = new Receiver(this);
            _KnownBluetoothDevices = new List<BluetoothDeviceWrapper>();
            DroidBluetoothManager = Application.Context.GetSystemService(Context.BluetoothService) as Android.Bluetooth.BluetoothManager;
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

        public event RemoteX.Bluetooth.BluetoothScanResultHandler OnDevicesFound;
        public event RemoteX.Bluetooth.BluetoothStartEndScanHandler OnDiscoveryFinished;
        public event RemoteX.Bluetooth.BluetoothStartEndScanHandler OnDiscoveryStarted;

        public void SearchForBlutoothDevices()
        {
            IntentFilter startFilter = new IntentFilter(BluetoothAdapter.ActionDiscoveryStarted);
            IntentFilter foundFilter = new IntentFilter(BluetoothDevice.ActionFound);
            IntentFilter finshFilter = new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished);

            Application.Context.RegisterReceiver(_DiscoveryStartedReceiver, startFilter);
            Application.Context.RegisterReceiver(_DevicesFoundReceiver, foundFilter);
            Application.Context.RegisterReceiver(_DiscoveryFinishedReceiver, finshFilter);

            BluetoothAdapter.StartDiscovery();
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

        public IBluetoothLEScanner LEScanner => throw new NotImplementedException();

        private class Receiver : BroadcastReceiver
        {
            BluetoothManager _BluetoothManager;
            public Receiver(BluetoothManager bluetoothManager)
            {
                this._BluetoothManager = bluetoothManager;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;
                if (BluetoothAdapter.ActionDiscoveryStarted == action)
                {
                    this._BluetoothManager.IsDiscoverying = true;
                    _BluetoothManager.OnDiscoveryStarted?.Invoke(_BluetoothManager);
                    Application.Context.UnregisterReceiver(this);
                }
                if (BluetoothDevice.ActionFound == action)
                {
                    BluetoothDevice device = intent.GetParcelableExtra(BluetoothDevice.ExtraDevice) as BluetoothDevice;
                    BluetoothDeviceWrapper deviceWrapper = BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice(_BluetoothManager, device);

                    _BluetoothManager.OnDevicesFound?.Invoke(_BluetoothManager, new RemoteX.Bluetooth.IBluetoothDevice[] { deviceWrapper });
                }
                if (BluetoothAdapter.ActionDiscoveryFinished == action)
                {
                    _BluetoothManager.IsDiscoverying = false;
                    _BluetoothManager.OnDiscoveryFinished?.Invoke(_BluetoothManager);
                    Application.Context.UnregisterReceiver(this);
                    Application.Context.UnregisterReceiver(_BluetoothManager._DevicesFoundReceiver);
                }

            }
        }
    }
}