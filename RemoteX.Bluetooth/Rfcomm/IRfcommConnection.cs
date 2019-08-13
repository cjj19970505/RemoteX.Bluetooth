﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RemoteX.Bluetooth.Rfcomm
{
    public interface IRfcommConnection
    {
        IBluetoothDevice RemoteDevice { get; }
        Stream InputStream { get; }
        Stream OutputStream { get; }
    }
}
