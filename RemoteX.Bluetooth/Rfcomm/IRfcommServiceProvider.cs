using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Bluetooth.Rfcomm
{
    /// <summary>
    /// 表示本机的Rfcomm实例
    /// </summary>
    public interface IRfcommServiceProvider
    {
        IRfcommConnection[] Connections { get; }

        event EventHandler<IRfcommConnection> OnConnectionReceived;
        IBluetoothManager BluetoothManager { get; }
        Guid ServiceId { get; }
        void StartAdvertising();
    }

    
}
