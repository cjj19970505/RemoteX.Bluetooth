using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt.Client
{
    public struct GattWriteResult
    {
        public GattCommunicationStatus CommunicationStatus;
        public GattErrorCode ProtocolError;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("WriteResult:(" + "CommunicationStatus:" + Enum.GetName(typeof(GattCommunicationStatus), CommunicationStatus));
            if(CommunicationStatus == GattCommunicationStatus.ProtocolError)
            {
                sb.Append(", ProtocolError:" + Enum.GetName(typeof(GattErrorCode), ProtocolError));
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
