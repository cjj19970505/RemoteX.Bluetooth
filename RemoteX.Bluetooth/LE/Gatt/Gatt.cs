using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public enum GattServiceType { Primary, Secondary }

    public struct GattPermissions
    {
        public bool Read;
        public bool Write;
    }

    public enum GattErrorCode
    {
        Success = 0,
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

    public enum GattRequestState
    {
        Pending = 0,
        Completed = 1,
        Canceled = 2
    }

    /// <summary>
    /// 3.3.1.1
    /// The Characteristic Properties bit field determines how the Characteristic Value can be used, or how the characteristic descriptors (see Section 3.3.3) can be accessed. If the bits defined in Table 3 5 are set, the action described is permitted. Multiple Characteristic Properties can be set.
    /// </summary>
    public struct GattCharacteristicProperties
    {
        public bool Broadcast;
        public bool Read;
        public bool WriteWithoutResponse;
        public bool Write;
        public bool Notify;
        public bool Indicate;
        public bool AuthenticatedSignedWrites;
        public bool ExtendedProperties;
    }
}
