using RemoteX.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using RemoteX.Bluetooth.Win10;

namespace RemoteX.Bluetooth.Win10.LE
{
    internal class RXBluetoothLEScanner:IBluetoothLEScanner
    {
        public DeviceWatcher BleDeviceWatcher { get; }

        public BluetoothLEScannerState Status
        {
            get
            {
                return BleDeviceWatcher.Status.ToRXScannerState();
            }
        }

        public event EventHandler<IBluetoothDevice> Added;
        public event EventHandler<IBluetoothDevice> Removed;
        public event EventHandler Stopped;
        public event EventHandler Updated;
        public event EventHandler EnumerationCompleted;

        public RXBluetoothLEScanner()
        {
            // Query for extra properties you want returned
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            BleDeviceWatcher =
                        DeviceInformation.CreateWatcher(
                                BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                                requestedProperties,
                                DeviceInformationKind.AssociationEndpoint);
            BleDeviceWatcher.Added += BleDeviceWatcher_Added;
            BleDeviceWatcher.EnumerationCompleted += BleDeviceWatcher_EnumerationCompleted;
            BleDeviceWatcher.Removed += BleDeviceWatcher_Removed;
            BleDeviceWatcher.Stopped += BleDeviceWatcher_Stopped;
            
        }

        private void BleDeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_ADDED:: Name:"+args.Name+" Address:"+BluetoothUtils.GetAddressStringFromDeviceId(args.Id));
            
            
        }

        private void BleDeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_STOPED");
            Stopped?.Invoke(this, null);
        }

        private void BleDeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_REMOVED:: Id:"+args.Id);
        }

        private void BleDeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_EnumerationCompleted");
            EnumerationCompleted?.Invoke(this, null);
        }

        public void Start()
        {
            BleDeviceWatcher.Start();
            
        }

        public void Stop()
        {
            BleDeviceWatcher.Stop();
        }

        
        
    }


}
