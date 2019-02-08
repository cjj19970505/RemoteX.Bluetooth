using RemoteX.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace RemoteX.Bluetooth.Win10
{
    public static class RXBluetoothUtils
    {
        public static DeviceWatcherStatus ToDeviceWatcherStatus(this BluetoothLEScannerState self)
        {
            switch (self)
            {
                case BluetoothLEScannerState.Created:
                    return DeviceWatcherStatus.Created;
                case BluetoothLEScannerState.Started:
                    return DeviceWatcherStatus.Started;
                case BluetoothLEScannerState.EnumerationCompleted:
                    return DeviceWatcherStatus.EnumerationCompleted;
                case BluetoothLEScannerState.Stopping:
                    return DeviceWatcherStatus.Stopping;
                case BluetoothLEScannerState.Stopped:
                    return DeviceWatcherStatus.Stopped;
                case BluetoothLEScannerState.Aborted:
                    return DeviceWatcherStatus.Aborted;

            }
            throw new Exception("No Matched DeviceWatcherStatus");
        }

        public static BluetoothLEScannerState ToRXScannerState(this DeviceWatcherStatus self)
        {
            switch(self)
            {
                case DeviceWatcherStatus.Created:
                    return BluetoothLEScannerState.Created;
                case DeviceWatcherStatus.Started:
                    return BluetoothLEScannerState.Started;
                case DeviceWatcherStatus.EnumerationCompleted:
                    return BluetoothLEScannerState.EnumerationCompleted;
                case DeviceWatcherStatus.Stopping:
                    return BluetoothLEScannerState.Stopping;
                case DeviceWatcherStatus.Stopped:
                    return BluetoothLEScannerState.Stopped;
                case DeviceWatcherStatus.Aborted:
                    return BluetoothLEScannerState.Aborted;

            }
            throw new Exception("No Matched BluetoothLEScannerState");
        }

        public static string GetAddressStringFromDeviceId(string deviceId)
        {
            return deviceId.Substring(deviceId.Length - 17);
        }

        public static Bluetooth.LE.Gatt.GattCharacteristicProperties ToRXProperties(this Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties self)
        {
            Bluetooth.LE.Gatt.GattCharacteristicProperties rxProperties = new Bluetooth.LE.Gatt.GattCharacteristicProperties();
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.AuthenticatedSignedWrites))
            {
                rxProperties.AuthenticatedSignedWrites = true;
            }
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Broadcast))
            {
                rxProperties.Broadcast = true;
            }
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.ExtendedProperties))
            {
                throw new NotImplementedException("NOT IMPLEMENT ExtendedProperties");
            }
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Indicate))
            {
                rxProperties.Indicate = true;
            }
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Notify))
            {
                rxProperties.Notify = true;
            }
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Read))
            {
                rxProperties.Read = true;
            }
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.ReliableWrites))
            {
                throw new NotImplementedException("NOT IMPLEMENT ReliableWrites");
            }
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.WritableAuxiliaries))
            {
                throw new NotImplementedException("NOT IMPLEMENT WritableAuxiliaries");
            }
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.Write))
            {
                rxProperties.Write = true;
            }
            if (self.HasFlag(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristicProperties.WriteWithoutResponse))
            {
                rxProperties.WriteWithoutResponse = true;
            }
            return rxProperties;
        }

        public static RemoteX.Bluetooth.LE.Gatt.GattCommunicationStatus ToRXCommunicationStatus(this Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus self)
        {
            return (RemoteX.Bluetooth.LE.Gatt.GattCommunicationStatus)self;
        }

        public static Bluetooth.LE.Gatt.Client.GattWriteResult ToRXGattWriteResult(this Windows.Devices.Bluetooth.GenericAttributeProfile.GattWriteResult self)
        {
            Bluetooth.LE.Gatt.Client.GattWriteResult rxResult = new Bluetooth.LE.Gatt.Client.GattWriteResult();
            rxResult.CommunicationStatus = self.Status.ToRXCommunicationStatus();
            if(rxResult.CommunicationStatus == Bluetooth.LE.Gatt.GattCommunicationStatus.Success)
            {
                rxResult.ProtocolError = Bluetooth.LE.Gatt.GattErrorCode.Success;
            }
            else if(rxResult.CommunicationStatus == Bluetooth.LE.Gatt.GattCommunicationStatus.ProtocolError)
            {
                rxResult.ProtocolError = (Bluetooth.LE.Gatt.GattErrorCode)self.ProtocolError;
            }
            return rxResult;
            
        }
    }
}
