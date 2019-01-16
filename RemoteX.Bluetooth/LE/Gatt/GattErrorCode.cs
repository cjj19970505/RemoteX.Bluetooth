using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public enum GattErrorCode
    {
        //
        // 摘要:
        //     GATT read operation is not permitted
        ReadNotPermitted = 2,
        //
        // 摘要:
        //     GATT write operation is not permitted
        WriteNotPermitted = 3,
        //
        // 摘要:
        //     Insufficient authentication for a given operation
        InsufficientAuthentication = 5,
        //
        // 摘要:
        //     The given request is not supported
        RequestNotSupported = 6,
        //
        // 摘要:
        //     A read or write operation was requested with an invalid offset
        InvalidOffset = 7,
        //
        // 摘要:
        //     A write operation exceeds the maximum length of the attribute
        InvalidAttributeLength = 13,
        //
        // 摘要:
        //     Insufficient encryption for a given operation
        InsufficientEncryption = 15,
        //
        // 摘要:
        //     To be added.
        ConnectionCongested = 143,
        //
        // 摘要:
        //     A GATT operation failed, errors other than the above
        Failure = 257
    }
}
