using RemoteX.Bluetooth.Rfcomm;
using RemoteX.Bluetooth.Win10.LE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;

namespace RemoteX.Bluetooth.Win10.Rfcomm
{
    internal class RXBluetoothRfcommScanner : IBluetoothRfcommScanner
    {
        public BluetoothManager BluetoothManager { get; }
        public DeviceWatcher DeviceWatcher { get; }
        public BluetoothRfcommScannerState Status
        {
            get
            {
                return DeviceWatcher.Status.ToRfcommScannerState();
            }
        }

        public event EventHandler<IBluetoothDevice> Added;
        public event EventHandler<IBluetoothDevice> Removed;
        public event EventHandler Stopped;
        public event EventHandler Updated;
        public event EventHandler EnumerationCompleted;

        public RXBluetoothRfcommScanner(BluetoothManager bluetoothManager)
        {
            BluetoothManager = bluetoothManager;
            string[] requestedProperties = new string[] { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            DeviceWatcher = DeviceInformation.CreateWatcher("(System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\")",
                                                            requestedProperties,
                                                            DeviceInformationKind.AssociationEndpoint);
            DeviceWatcher.Added += DeviceWatcher_Added;
            DeviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            DeviceWatcher.Removed += DeviceWatcher_Removed;
            DeviceWatcher.Stopped += DeviceWatcher_Stopped;
            DeviceWatcher.Updated += DeviceWatcher_Updated;
            
        }

        private void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            System.Diagnostics.Debug.WriteLine("Update:" + args.Id);
        }

        private void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_STOPED");
            var invokeDispatcherAction = (BluetoothManager as BluetoothManager).Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Stopped?.Invoke(this, null);
            });
        }

        private void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_REMOVED:: Id:" + args.Id);
            var device = (BluetoothManager as BluetoothManager).RemoveBluetoothDeviceFromDeviceInformationUpdate(args);

            var invokeDispatcherAction = (BluetoothManager as BluetoothManager).Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Removed?.Invoke(this, device);
            });
        }

        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_EnumerationCompleted");
            EnumerationCompleted?.Invoke(this, null);
        }

        private void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            System.Diagnostics.Debug.WriteLine("BLEWATCHER_ADDED:: Name:" + args.Name + " Address:" + RXBluetoothUtils.GetAddressStringFromDeviceId(args.Id));
            RXBluetoothDevice device = (BluetoothManager as BluetoothManager).GetBluetoothDeviceFromDeviceInformation(args);
            var invokeDispatcherAction = (BluetoothManager as BluetoothManager).Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Added?.Invoke(this, device);
            });
        }

        public void Start()
        {
            DeviceWatcher.Start();
        }

        public void Stop()
        {
            DeviceWatcher.Stop();
        }
    }
}
