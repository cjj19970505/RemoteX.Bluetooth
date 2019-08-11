using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.Rfcomm
{
    public enum BluetoothRfcommScannerState { Created, Started, EnumerationCompleted, Stopping, Stopped, Aborted }

    public interface IBluetoothRfcommScanner
    {
        BluetoothRfcommScannerState Status { get; }
        event EventHandler<IBluetoothDevice> Added;
        event EventHandler<IBluetoothDevice> Removed;
        event EventHandler Stopped;
        event EventHandler Updated;
        event EventHandler EnumerationCompleted;
        void Start();
        void Stop();
    }
}
