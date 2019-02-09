using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Bluetooth.LE.Gatt.Client
{
    public interface IClientCharacteristicConfiguration
    {
        Task<GattWriteResult> SetValueAsync(bool notification, bool indication);
    }
}
