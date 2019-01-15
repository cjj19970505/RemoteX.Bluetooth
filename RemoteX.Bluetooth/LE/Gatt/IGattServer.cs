using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public interface IGattServer
    {
        ulong Address { get; }
        IGattServerService[] Services { get; }
        void AddService(IGattServerService service);
        void StartAdvertising();
        void NotifyTest();
        bool IsSupported { get; }

        //GattServer.DroidGattServer.SendResponse(device, requestId, GattStatus.Success, offset, Encoding.Default.GetBytes("XJSayHello"+requestId));

        /// <summary>
        /// 以后看看有没有更好安排这货的地方
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        void SendResponse(IBluetoothDevice bluetoothDevice, int requestId, byte[] responseBytes);
    }
}
