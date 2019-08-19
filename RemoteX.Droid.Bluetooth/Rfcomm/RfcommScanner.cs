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
using RemoteX.Bluetooth.Rfcomm;
using static RemoteX.Bluetooth.Droid.BluetoothManager;

namespace RemoteX.Bluetooth.Droid.Rfcomm
{
    class RfcommScanner : IBluetoothRfcommScanner
    {
        public BluetoothRfcommScannerState Status { get; private set; }

        public event EventHandler<IBluetoothDevice> Added;
        public event EventHandler<IBluetoothDevice> Removed;
        public event EventHandler Stopped;
        public event EventHandler Updated;
        public event EventHandler EnumerationCompleted;

        Receiver DiscoveryStartedReceiver;
        Receiver DevicesFoundReceiver;
        Receiver DiscoveryFinishedReceiver;

        public BluetoothManager BluetoothManager { get; }

        public RfcommScanner(BluetoothManager bluetoothManager)
        {
            Status = BluetoothRfcommScannerState.Created;
            BluetoothManager = bluetoothManager;
            DiscoveryStartedReceiver = new Receiver(this);
            DevicesFoundReceiver = new Receiver(this);
            DiscoveryFinishedReceiver = new Receiver(this);


        }

        public void Start()
        {
            Status = BluetoothRfcommScannerState.Started;
            IntentFilter startFilter = new IntentFilter(Android.Bluetooth.BluetoothAdapter.ActionDiscoveryStarted);
            IntentFilter foundFilter = new IntentFilter(Android.Bluetooth.BluetoothDevice.ActionFound);
            IntentFilter finishFilter = new IntentFilter(Android.Bluetooth.BluetoothAdapter.ActionDiscoveryFinished);

            Application.Context.RegisterReceiver(DiscoveryStartedReceiver, startFilter);
            Application.Context.RegisterReceiver(DevicesFoundReceiver, foundFilter);
            Application.Context.RegisterReceiver(DiscoveryFinishedReceiver, finishFilter);

            BluetoothManager.BluetoothAdapter.StartDiscovery();

        }

        public void Stop()
        {
            if (Status == BluetoothRfcommScannerState.Started)
            {
                BluetoothManager.BluetoothAdapter.CancelDiscovery();

                //Application.Context.UnregisterReceiver(DiscoveryStartedReceiver);
                Application.Context.UnregisterReceiver(DevicesFoundReceiver);
                Application.Context.UnregisterReceiver(DiscoveryFinishedReceiver);
            }
            Status = BluetoothRfcommScannerState.Stopping;
            Status = BluetoothRfcommScannerState.Stopped;
            Stopped?.Invoke(this, null);

        }

        public class Receiver : BroadcastReceiver
        {
            RfcommScanner RfcommScanner { get; }

            public Receiver(RfcommScanner rfcommScanner)
            {
                RfcommScanner = rfcommScanner;
            }
            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;
                if (action == Android.Bluetooth.BluetoothAdapter.ActionDiscoveryStarted)
                {
                    Application.Context.UnregisterReceiver(this);
                }
                if (action == Android.Bluetooth.BluetoothDevice.ActionFound)
                {
                    Android.Bluetooth.BluetoothDevice droidDevice = intent.GetParcelableExtra(Android.Bluetooth.BluetoothDevice.ExtraDevice) as Android.Bluetooth.BluetoothDevice;
                    var deviceWrapper = BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice(RfcommScanner.BluetoothManager, droidDevice);
                    RfcommScanner.Added?.Invoke(RfcommScanner, deviceWrapper);
                }
                if (action == Android.Bluetooth.BluetoothAdapter.ActionDiscoveryFinished)
                {
                    Application.Context.UnregisterReceiver(this);
                    Application.Context.UnregisterReceiver(RfcommScanner.DevicesFoundReceiver);
                    RfcommScanner.Status = BluetoothRfcommScannerState.EnumerationCompleted;
                    RfcommScanner.EnumerationCompleted?.Invoke(RfcommScanner, null);
                }
            }
        }
    }
}