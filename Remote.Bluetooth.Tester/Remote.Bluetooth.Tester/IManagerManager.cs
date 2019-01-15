using RemoteX.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Remote.Bluetooth.Tester
{
    public interface IManagerManager
    {
        IBluetoothManager BluetoothManager { get; }
    }
}
