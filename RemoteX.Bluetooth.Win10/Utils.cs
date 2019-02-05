using RemoteX.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace RemoteX.Bluetooth.Win10
{
    public static class BluetoothUtils
    {
        public static DeviceWatcherStatus ToDeviceWatcherStatus(this BluetoothLEScannerState self)
        {
            switch (self)
            {
                case BluetoothLEScannerState.Created:
                    return DeviceWatcherStatus.Created;
                case BluetoothLEScannerState.Started:
                    return DeviceWatcherStatus.Started;
                case BluetoothLEScannerState.EnumerationCompleted:
                    return DeviceWatcherStatus.EnumerationCompleted;
                case BluetoothLEScannerState.Stopping:
                    return DeviceWatcherStatus.Stopping;
                case BluetoothLEScannerState.Stopped:
                    return DeviceWatcherStatus.Stopped;
                case BluetoothLEScannerState.Aborted:
                    return DeviceWatcherStatus.Aborted;

            }
            throw new Exception("No Matched DeviceWatcherStatus");
        }

        public static BluetoothLEScannerState ToRXScannerState(this DeviceWatcherStatus self)
        {
            switch(self)
            {
                case DeviceWatcherStatus.Created:
                    return BluetoothLEScannerState.Created;
                case DeviceWatcherStatus.Started:
                    return BluetoothLEScannerState.Started;
                case DeviceWatcherStatus.EnumerationCompleted:
                    return BluetoothLEScannerState.EnumerationCompleted;
                case DeviceWatcherStatus.Stopping:
                    return BluetoothLEScannerState.Stopping;
                case DeviceWatcherStatus.Stopped:
                    return BluetoothLEScannerState.Stopped;
                case DeviceWatcherStatus.Aborted:
                    return BluetoothLEScannerState.Aborted;

            }
            throw new Exception("No Matched BluetoothLEScannerState");
        }

        public static string GetAddressStringFromDeviceId(string deviceId)
        {
            return deviceId.Substring(deviceId.Length - 17);
        }
    }
}
