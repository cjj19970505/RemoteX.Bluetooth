using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public interface IGattService
    {
        GattServiceType ServiceType { get; }
        Guid Uuid { get; }
        
    }
    
}
