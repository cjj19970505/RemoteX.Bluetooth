using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt.Client
{
    public struct GattServiceResult
    {
        public GattErrorCode ProtocolError;
        public IGattClientService[] Services;
    }
}
