using System;
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
        Stream InputStream { get; }
        Stream OutputStream { get; }
        Task ConnectAsync();

        Task TrySend();
    }
}
