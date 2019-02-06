using System;
using System.Collections.Generic;
using System.Text;
using RemoteX.Bluetooth.LE;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;

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
        /// 本机是否支持蓝牙
        /// </summary>
        bool Supported { get; }

        IBluetoothDevice GetBluetoothDevice(ulong macAddress);

        //Ble Zone
        IGattServer GattSever { get; }

        IGattServiceBuilder NewGattServiceBuilder();
        IGattCharacteristicBuilder NewGattCharacteristicBuilder();
        IGattDescriptorBuilder NewGattDescriptorBuilder();

        IBluetoothLEScanner LEScanner { get; }

    }
}
