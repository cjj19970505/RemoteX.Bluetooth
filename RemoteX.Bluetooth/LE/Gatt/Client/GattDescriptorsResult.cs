using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt.Client
{
    public struct GattDescriptorsResult
    {
        public GattCommunicationStatus CommunicationStatus;
        public GattErrorCode ProtocolError;
        public IGattClientDescriptor[] Descriptors;
    }
}
