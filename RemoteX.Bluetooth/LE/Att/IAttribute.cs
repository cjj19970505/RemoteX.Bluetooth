using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Att
{
    public interface IAttribute
    {
        ushort AttributeHandle { get; }
    }
}
