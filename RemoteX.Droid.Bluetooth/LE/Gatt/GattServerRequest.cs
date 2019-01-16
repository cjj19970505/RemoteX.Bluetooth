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
    }

    class DescriptorReadRequest : IDescriptorReadRequest
    {
        public IGattServerDescriptor TargetDescriptor { get; set; }

        public IBluetoothDevice SourceDevice { get; set; }

        public int RequestId { get; set; }

        public byte[] Value { get; set; }

        public int Offset { get; set; }
    }

    class CharacteristicWriteRequest : ICharacteristicWriteRequest
    {

        public IGattServerCharacteristic TargetCharacteristic { get; set; }

        public bool ResponseNeeded { get; set; }

        public int Offset { get; set; }

        public IBluetoothDevice SourceDevice { get; set; }

        public int RequestId { get; set; }

        public byte[] Value { get; set; }
    }

    class DescriptorWriteRequest : IDescriptorWriteRequest
    {

        public IGattServerDescriptor TargetDescriptor { get; set; }

        public bool ResponseNeeded { get; set; }

        public int Offset { get; set; }

        public IBluetoothDevice SourceDevice { get; set; }

        public int RequestId { get; set; }

        public byte[] Value { get; set; }
    }
}