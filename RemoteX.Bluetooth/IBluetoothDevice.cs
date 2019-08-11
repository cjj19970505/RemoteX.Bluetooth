using RemoteX.Bluetooth.LE.Gatt.Client;
using RemoteX.Bluetooth.Rfcomm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        /// 获取连接到该设备GattServer的接口
        /// </summary>
        IGattClient GattClient { get; }
        Task RfcommConnectAsync();
        Task<RfcommDeviceServiceResult> GetRfcommServicesAsync();
        Task<RfcommDeviceServiceResult> GetRfcommServicesForIdAsync(Guid serviceId);
    }
}
