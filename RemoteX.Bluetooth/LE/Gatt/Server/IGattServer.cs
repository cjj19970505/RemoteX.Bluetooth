using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt.Server
{
    public interface IGattServer
    {
        event EventHandler<IGattServerService> OnServiceAdded;
        ulong Address { get; }
        IGattServerService[] Services { get; }
        bool IsSupported { get; }
        IBluetoothDevice[] ConnectedDevices { get; }
        event EventHandler<IBluetoothDevice> DeviceConnected;
        event EventHandler<IBluetoothDevice> DeviceDisconnected;
        void AddService(IGattServerService service);
        void StartAdvertising();
        void NotifyTest();
        
        
    }
}
