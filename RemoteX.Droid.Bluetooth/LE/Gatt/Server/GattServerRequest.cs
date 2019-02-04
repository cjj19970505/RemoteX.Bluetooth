using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;

namespace RemoteX.Droid.Bluetooth.LE.Gatt.Server
{
    class CharacteristicReadRequest : ICharacteristicReadRequest
    {
        public IGattServerCharacteristic TargetCharacteristic { get; set; }

        public int Offset { get; set; }

        public IBluetoothDevice SourceDevice { get; set; }

        public int RequestId { get; set; }

        public byte[] Value { get; set; }

        public GattRequestState State { get; private set; }

        public CharacteristicReadRequest()
        {
            State = GattRequestState.Pending;
        }

        public event EventHandler<GattRequestState> StateChanged;

        public void RespondWithValue(byte[] value)
        {
            (TargetCharacteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, Android.Bluetooth.GattStatus.Success, 0, value);
            State = GattRequestState.Completed;
            StateChanged?.Invoke(this, GattRequestState.Pending);
        }

        public void RespondWithProtocolError(GattErrorCode errorCode)
        {
            (TargetCharacteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, (Android.Bluetooth.GattStatus)errorCode, 0, null);
            State = GattRequestState.Canceled;
            StateChanged?.Invoke(this, GattRequestState.Pending);
        }
    }

    class DescriptorReadRequest : IDescriptorReadRequest
    {
        public IGattServerDescriptor TargetDescriptor { get; set; }

        public IBluetoothDevice SourceDevice { get; set; }

        public int RequestId { get; set; }

        public byte[] Value { get; set; }

        public int Offset { get; set; }

        public GattRequestState State { get; private set; }

        public DescriptorReadRequest()
        {
            State = GattRequestState.Pending;
        }

        public event EventHandler<GattRequestState> StateChanged;

        public void RespondWithValue(byte[] value)
        {
            (TargetDescriptor.Characteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, Android.Bluetooth.GattStatus.Success, 0, value);
            State = GattRequestState.Completed;
            StateChanged?.Invoke(this, GattRequestState.Pending);
        }

        /// <summary>
        /// 不知道这个是要把State设置成Cancelled还是Completed
        /// </summary>
        /// <param name="errorCode"></param>
        public void RespondWithProtocolError(GattErrorCode errorCode)
        {
            (TargetDescriptor.Characteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, (Android.Bluetooth.GattStatus)errorCode, 0, null);
            State = GattRequestState.Canceled;
            StateChanged?.Invoke(this, GattRequestState.Pending);
        }
    }

    class CharacteristicWriteRequest : ICharacteristicWriteRequest
    {

        public IGattServerCharacteristic TargetCharacteristic { get; set; }

        public bool ResponseNeeded { get; set; }

        public int Offset { get; set; }

        public IBluetoothDevice SourceDevice { get; set; }

        public int RequestId { get; set; }

        public byte[] Value { get; set; }

        public GattRequestState State { get; private set; }

        public CharacteristicWriteRequest()
        {
            if (ResponseNeeded)
            {
                State = GattRequestState.Pending;
            }
            else
            {
                State = GattRequestState.Completed;
            }
            
        }

        public event EventHandler<GattRequestState> StateChanged;

        public void RespondSuccess()
        {
            (TargetCharacteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, Android.Bluetooth.GattStatus.Success, 0, null);
            State = GattRequestState.Completed;
            StateChanged?.Invoke(this, GattRequestState.Pending);
        }

        public void RespondWithProtocolError(GattErrorCode errorCode)
        {
            (TargetCharacteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, (Android.Bluetooth.GattStatus)errorCode, 0, null);
            State = GattRequestState.Canceled;
            StateChanged?.Invoke(this, GattRequestState.Pending);
        }
    }

    class DescriptorWriteRequest : IDescriptorWriteRequest
    {

        public IGattServerDescriptor TargetDescriptor { get; set; }

        public bool ResponseNeeded { get; set; }

        public int Offset { get; set; }

        public IBluetoothDevice SourceDevice { get; set; }

        public int RequestId { get; set; }

        public byte[] Value { get; set; }

        public GattRequestState State { get; private set; }

        public DescriptorWriteRequest()
        {
            if (ResponseNeeded)
            {
                State = GattRequestState.Pending;
            }
            else
            {
                State = GattRequestState.Completed;
            }
        }

        public event EventHandler<GattRequestState> StateChanged;

        public void RespondSuccess()
        {
            (TargetDescriptor.Characteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, Android.Bluetooth.GattStatus.Success, 0, null);
            State = GattRequestState.Completed;
            StateChanged?.Invoke(this, GattRequestState.Pending);
        }

        public void RespondWithProtocolError(GattErrorCode errorCode)
        {
            (TargetDescriptor.Characteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, (Android.Bluetooth.GattStatus)errorCode, 0, null);
            State = GattRequestState.Canceled;
            StateChanged?.Invoke(this, GattRequestState.Pending);
        }
    }
}