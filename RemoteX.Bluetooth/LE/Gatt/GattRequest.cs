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
        void RespondWithProtocolError(GattErrorCode errorCode);
    }
    public interface ICharacteristicReadRequest: IGattServerRequest
    {
        IGattServerCharacteristic TargetCharacteristic { get; }
        int Offset { get; }
        void RespondWithValue(byte[] value);
    }

    public interface IDescriptorReadRequest: IGattServerRequest
    {
        IGattServerDescriptor TargetDescriptor { get; }
        int Offset { get; }
        void RespondWithValue(byte[] value);
    }

    public interface ICharacteristicWriteRequest:IGattServerRequest
    {
        IGattServerCharacteristic TargetCharacteristic { get; }
        //bool PreparedWrite;
        bool ResponseNeeded { get; }
        int Offset { get; }
        void RespondSuccess();
    }

    public interface IDescriptorWriteRequest : IGattServerRequest
    {
        IGattServerDescriptor TargetDescriptor { get; }
        bool ResponseNeeded { get; }
        int Offset { get; }
        void RespondSuccess();
    }
}
