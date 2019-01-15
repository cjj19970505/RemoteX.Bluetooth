using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth
{
    public delegate void BluetoothDeviceGetUuidsHanlder(IBluetoothDevice bluetoothDevice, Guid[] guids);

    /// <summary>
    /// 目前不保证在不同的地方获取相同的一个设备时，两个IBluetoothDevice实例引用相同
    /// 但是以后要保证哦
    /// </summary>
    public interface IBluetoothDevice
    {
        /// <summary>
        /// 设备名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 设备的mac地址
        /// </summary>
        ulong Address { get; }

        /// <summary>
        /// 最新获取的Uuid
        /// </summary>
        Guid[] LastestFetchedUuids { get; }

        /// <summary>
        /// 查询是否正在获取Uuid
        /// </summary>
        bool IsFetchingUuids { get; }

        /// <summary>
        /// 当获取到Uuid使触发该事件
        /// </summary>
        event BluetoothDeviceGetUuidsHanlder OnUuidsFetched;

        /// <summary>
        /// 获取uuid，若获取成功会触发<c>onUuidsFetched</c>
        /// 若当前正在查询Uuid（即IsFetchingUuids == true），则什么都不做
        /// </summary>
        void FetchUuidsWithSdp();

        /// <summary>
        /// 其实这个不能真正阻止FetchingUuid，只能关闭发现，撤销Receiver之类的
        /// </summary>
        void stopFetchingUuidsWithSdp();
    }
}
