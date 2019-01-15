using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public interface IGattDescriptor
    {
        IGattServerCharacteristic Characteristic { get; }
        Guid Uuid { get; }
    }
}
