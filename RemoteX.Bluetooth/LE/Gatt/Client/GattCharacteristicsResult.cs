﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt.Client
{
    public struct GattCharacteristicsResult
    {
        public GattCommunicationStatus CommunicationStatus;
        public GattErrorCode ProtocolError;
        public IGattClientCharacteristic[] Characteristics;
    }
}
