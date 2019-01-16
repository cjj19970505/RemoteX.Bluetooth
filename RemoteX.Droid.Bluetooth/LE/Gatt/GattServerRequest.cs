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

namespace RemoteX.Droid.Bluetooth.LE.Gatt
{
    class CharacteristicReadRequest : ICharacteristicReadRequest
    {
        public IGattServerCharacteristic TargetCharacteristic { get; set; }

        public int Offset { get; set; }

        public IBluetoothDevice SourceDevice { get; set; }

        public int RequestId { get; set; }

        public byte[] Value { get; set; }

        public void RespondWithValue(byte[] value)
        {
            (TargetCharacteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, Android.Bluetooth.GattStatus.Success, 0, value);
        }

        public void RespondWithProtocolError(GattErrorCode errorCode)
        {
            (TargetCharacteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, (Android.Bluetooth.GattStatus)errorCode, 0, null);
        }
    }

    class DescriptorReadRequest : IDescriptorReadRequest
    {
        public IGattServerDescriptor TargetDescriptor { get; set; }

        public IBluetoothDevice SourceDevice { get; set; }

        public int RequestId { get; set; }

        public byte[] Value { get; set; }

        public int Offset { get; set; }

        public void RespondWithValue(byte[] value)
        {
            (TargetDescriptor.Characteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, Android.Bluetooth.GattStatus.Success, 0, value);
        }

        public void RespondWithProtocolError(GattErrorCode errorCode)
        {
            (TargetDescriptor.Characteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, (Android.Bluetooth.GattStatus)errorCode, 0, null);
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

        public void RespondSuccess()
        {
            (TargetCharacteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, Android.Bluetooth.GattStatus.Success, 0, null);
        }

        public void RespondWithProtocolError(GattErrorCode errorCode)
        {
            (TargetCharacteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, (Android.Bluetooth.GattStatus)errorCode, 0, null);
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

        public void RespondSuccess()
        {
            (TargetDescriptor.Characteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, Android.Bluetooth.GattStatus.Success, 0, null);
        }

        public void RespondWithProtocolError(GattErrorCode errorCode)
        {
            (TargetDescriptor.Characteristic.Service.Server as GattServer).DroidGattServer.SendResponse((SourceDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, RequestId, (Android.Bluetooth.GattStatus)errorCode, 0, null);
        }
    }
}