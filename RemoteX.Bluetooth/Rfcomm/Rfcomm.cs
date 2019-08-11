using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.Rfcomm
{
    //
    // 摘要:
    //     指定常见的 Bluetooth 错误情况。
    public enum BluetoothError
    {
        //
        // 摘要:
        //     操作已成功完成或维护。
        Success = 0,
        //
        // 摘要:
        //     Bluetooth 无线收发器不可用。 关闭 Bluetooth 无线收发器后将出现此错误。
        RadioNotAvailable = 1,
        //
        // 摘要:
        //     此操作无法维护，因为必要资源当前正在使用中。
        ResourceInUse = 2,
        //
        // 摘要:
        //     此操作无法完成，因为远程设备未连接。
        DeviceNotConnected = 3,
        //
        // 摘要:
        //     发生了意外错误。
        OtherError = 4,
        //
        // 摘要:
        //     此操作已由策略禁用。
        DisabledByPolicy = 5,
        //
        // 摘要:
        //     当前蓝牙无线模块硬件不支持该操作。
        NotSupported = 6,
        //
        // 摘要:
        //     用户已禁用此操作。
        DisabledByUser = 7,
        //
        // 摘要:
        //     该操作需要许可。
        ConsentRequired = 8,
        //
        // 摘要:
        //     不支持该传输。
        TransportNotSupported = 9
    }
    public class RfcommDeviceServiceResult
    {
        public BluetoothError Error { get; set; }
        public IReadOnlyList<IRfcommDeviceService> Services { get; set; }
    }
}
