using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public interface IGattServerRequest
    {
        IBluetoothDevice SourceDevice { get; }
        int RequestId { get; }
        byte[] Value { get; }
    }
    public interface ICharacteristicReadRequest: IGattServerRequest
    {
        IGattServerCharacteristic TargetCharacteristic { get; }
        int Offset { get; }
    }

    public interface IDescriptorReadRequest: IGattServerRequest
    {
        IGattServerDescriptor TargetDescriptor { get; }
        int Offset { get; }
    }

    public interface ICharacteristicWriteRequest:IGattServerRequest
    {
        IGattServerCharacteristic TargetCharacteristic { get; }
        //bool PreparedWrite;
        bool ResponseNeeded { get; }
        int Offset { get; }
    }

    public interface IDescriptorWriteRequest : IGattServerRequest
    {
        IGattServerDescriptor TargetDescriptor { get; }
        bool ResponseNeeded { get; }
        int Offset { get; }
    }
}
