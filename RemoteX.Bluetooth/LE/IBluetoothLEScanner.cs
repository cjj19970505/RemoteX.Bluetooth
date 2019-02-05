using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE
{
    public enum BluetoothLEScannerState { Created, Started, EnumerationCompleted, Stopping, Stopped, Aborted}
    /// <summary>
    /// 整个设计都要模仿DeviceWatcher
    /// https://docs.microsoft.com/en-us/uwp/api/Windows.Devices.Enumeration.DeviceWatcher
    /// </summary>
    public interface IBluetoothLEScanner
    {
        BluetoothLEScannerState Status { get; }

        event EventHandler<IBluetoothDevice> Added;
        event EventHandler<IBluetoothDevice> Removed;
        event EventHandler Stopped;
        event EventHandler Updated;
        event EventHandler EnumerationCompleted;
        
        void Start();
        void Stop();
        
        
    }

}
