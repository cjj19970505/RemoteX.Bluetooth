using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public enum GattCommunicationStatus
    {
        //
        // 摘要:
        //     操作成功完成。
        Success = 0,
        //
        // 摘要:
        //     此时无法与该设备进行通信。
        Unreachable = 1,
        //
        // 摘要:
        //     出现了 GATT 通信协议错误。
        ProtocolError = 2,
        //
        // 摘要:
        //     拒绝访问。
        AccessDenied = 3
    }
}
