using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public enum GattRequestState
    {
        Pending = 0,
        Completed = 1,
        Canceled = 2
    }
    public interface IGattServerRequest
    {
        IBluetoothDevice SourceDevice { get; }
        int RequestId { get; }
        byte[] Value { get; }
        GattRequestState State { get; }
        void RespondWithProtocolError(GattErrorCode errorCode);

        /// <summary>
        /// Second Arg is previous state
        /// current state can be seen in State Property
        /// </summary>
        event EventHandler<GattRequestState> StateChanged;
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
