using System;
using System.Collections.Generic;
using System.Text;
using RemoteX.Bluetooth.LE.Gatt;

namespace RemoteX.Bluetooth
{
    public delegate void BluetoothScanResultHandler(IBluetoothManager bluetoothManager, IBluetoothDevice[] bluetoothDevices);
    public delegate void BluetoothStartEndScanHandler(IBluetoothManager bluetoothManager);

    /// <summary>
    /// 蓝牙管理器
    /// </summary>
    public interface IBluetoothManager
    {
        /// <summary>
        /// 查找设备过程中找到时触发
        /// </summary>
        event BluetoothScanResultHandler OnDevicesFound;

        /// <summary>
        /// 查找设备完成时触发
        /// </summary>
        event BluetoothStartEndScanHandler OnDiscoveryFinished;

        /// <summary>
        /// 查找设备开始时触发
        /// </summary>
        event BluetoothStartEndScanHandler OnDiscoveryStarted;

        /// <summary>
        /// 本机是否支持蓝牙
        /// </summary>
        bool Supported { get; }

        /// <summary>
        /// 开始查找设备
        /// 查找到的设备会通过onDeviceFound传出
        /// </summary>
        void SearchForBlutoothDevices();

        IBluetoothDevice[] PairedDevices { get; }

        IBluetoothDevice GetBluetoothDevice(ulong macAddress);

        //Ble Zone
        IGattServer GattSever { get; }

        IGattServiceBuilder NewGattServiceBuilder();
        IGattCharacteristicBuilder NewGattCharacteristicBuilder();
        IGattDescriptorBuilder NewGattDescriptorBuilder();



    }
}
