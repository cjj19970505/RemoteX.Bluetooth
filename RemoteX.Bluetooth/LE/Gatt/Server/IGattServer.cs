using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt.Server
{
    public interface IGattServer
    {
        ulong Address { get; }
        IGattServerService[] Services { get; }
        void AddService(IGattServerService service);
        void StartAdvertising();
        void NotifyTest();
        bool IsSupported { get; }
    }
}
