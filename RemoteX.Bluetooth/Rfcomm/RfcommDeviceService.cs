﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Bluetooth.Rfcomm
{
    public interface IRfcommDeviceService
    {
        Guid ServiceId { get; }
        IBluetoothDevice Device { get; }
        Task ConnectAsync();
        IRfcommConnection RfcommConnection { get; }
    }
}
