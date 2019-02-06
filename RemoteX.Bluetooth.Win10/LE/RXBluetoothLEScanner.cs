using RemoteX.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using RemoteX.Bluetooth.Win10;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;

namespace RemoteX.Bluetooth.Win10.LE
{
    internal class RXBluetoothLEScanner:IBluetoothLEScanner
    {
        public DeviceWatcher BleDeviceWatcher { get; }
        public IBluetoothManager BluetoothManager { get; }
        public Windows.UI.Core.CoreDispatcher Dispatcher { get; }

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

        public RXBluetoothLEScanner(BluetoothManager bluetoothManager)
        {
            BluetoothManager = bluetoothManager;
            Dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
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
            BleDeviceWatcher.Updated += BleDeviceWatcher_Updated;
            
        }

        private void BleDeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            throw new NotImplementedException();
        }

        private void BleDeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_ADDED:: Name:"+args.Name+" Address:"+RXBluetoothUtils.GetAddressStringFromDeviceId(args.Id));
            RXBluetoothDevice device = (BluetoothManager as BluetoothManager).GetBluetoothDeviceFromDeviceInformation(args);
            
            var invokeDispatcherAction = (BluetoothManager as BluetoothManager).Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Added?.Invoke(this, device);
            });
        }
   
        private void BleDeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_STOPED");
            var invokeDispatcherAction = (BluetoothManager as BluetoothManager).Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Stopped?.Invoke(this, null);
            });

        }

        private void BleDeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_REMOVED:: Id:"+args.Id);
            var device = (BluetoothManager as BluetoothManager).RemoveBluetoothDeviceFromDeviceInformationUpdate(args);

            var invokeDispatcherAction = (BluetoothManager as BluetoothManager).Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Removed?.Invoke(this, device);
            });
            
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
