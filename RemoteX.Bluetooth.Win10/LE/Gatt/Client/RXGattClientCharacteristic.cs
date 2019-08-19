using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Client;
using RemoteX.Bluetooth.LE.Gatt.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace RemoteX.Bluetooth.Win10.LE.Gatt.Client
{
    internal class RXGattClientCharacteristic : IGattClientCharacteristic
    {
        public IGattClientService Service => throw new NotImplementedException();

        public byte[] LatestValue => throw new NotImplementedException();

        public IGattServerDescriptor[] Descriptors => throw new NotImplementedException();

        public GattPermissions Permissions => throw new NotImplementedException();

        public Bluetooth.LE.Gatt.GattCharacteristicProperties CharacteristicProperties
        {
            get
            {
                return Win10Characteristic.CharacteristicProperties.ToRXProperties();
            }
        }

        public int CharacteristicValueHandle => throw new NotImplementedException();

        public Guid Uuid
        {
            get
            {
                return Win10Characteristic.Uuid;
            }
        }

        public Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic Win10Characteristic { get; }

        public ushort AttributeHandle
        {
            get
            {
                return Win10Characteristic.AttributeHandle;
            }
        }

        public event EventHandler<byte[]> OnNotified;

        public async Task<ReadCharacteristicValueResult> ReadCharacteristicValueAsync()
        {
            Windows.Devices.Bluetooth.GenericAttributeProfile.GattReadResult win10Result = await Win10Characteristic.ReadValueAsync();
            System.Diagnostics.Debug.WriteLine(win10Result.Status);
            ReadCharacteristicValueResult rxResult = new ReadCharacteristicValueResult();
            rxResult.CommunicationStatus = win10Result.Status.ToRXCommunicationStatus();
            if (rxResult.CommunicationStatus == GattCommunicationStatus.Success)
            {
                var reader = DataReader.FromBuffer(win10Result.Value);
                byte[] valueBytes = new byte[reader.UnconsumedBufferLength];
                reader.ReadBytes(valueBytes);
                rxResult.ProtocolError = GattErrorCode.Success;
                rxResult.Value = valueBytes;
            }
            else if (rxResult.CommunicationStatus == GattCommunicationStatus.ProtocolError)
            {
                rxResult.ProtocolError = (GattErrorCode)win10Result.ProtocolError;
            }
            else if(rxResult.CommunicationStatus == GattCommunicationStatus.Unreachable)
            {
                throw new NotImplementedException("UNREACHABLE");
            }
            else if(rxResult.CommunicationStatus == GattCommunicationStatus.AccessDenied)
            {
                throw new NotImplementedException("ACCESSDENIED");
            }
            return rxResult;
        }

        public RXGattClient RXGattClient { get; }

        public IClientCharacteristicConfiguration GattCharacteristicConfiguration { get; }

        public RXGattClientCharacteristic(RXGattClient rxGattClient, Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic win10Characteristic)
        {
            RXGattClient = rxGattClient;
            Win10Characteristic = win10Characteristic;
            GattCharacteristicConfiguration = new RXClientCharacteristicConfiguration(this);
            Win10Characteristic.ValueChanged += Win10Characteristic_ValueChanged;
            //Win10Characteristic.GetDescriptorsForUuidAsync()
            
        }

        private void Win10Characteristic_ValueChanged(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic sender, Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            byte[] valueBytes = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(valueBytes);
            OnNotified?.Invoke(this, valueBytes);
        }

        public async Task<Bluetooth.LE.Gatt.Client.GattDescriptorsResult> DiscoverAllCharacteristicDescriptorsAsync()
        {
            var win10Result = await Win10Characteristic.GetDescriptorsAsync();
            Bluetooth.LE.Gatt.Client.GattDescriptorsResult rxResult = new Bluetooth.LE.Gatt.Client.GattDescriptorsResult();
            rxResult.CommunicationStatus = win10Result.Status.ToRXCommunicationStatus();
            if (rxResult.CommunicationStatus == RemoteX.Bluetooth.LE.Gatt.GattCommunicationStatus.Success)
            {
                rxResult.ProtocolError = GattErrorCode.Success;
                List<RXGattClientDescriptor> rxDescriptorList = new List<RXGattClientDescriptor>();
                foreach(var win10Descriptor in win10Result.Descriptors)
                {
                    rxDescriptorList.Add(RXGattClient.GetRXDescriptorFromWin10Descriptor(win10Descriptor));
                }
                rxResult.Descriptors = rxDescriptorList.ToArray();
            }
            else if(rxResult.CommunicationStatus == RemoteX.Bluetooth.LE.Gatt.GattCommunicationStatus.ProtocolError)
            {
                rxResult.ProtocolError = (Bluetooth.LE.Gatt.GattErrorCode)(win10Result.ProtocolError);
            }
            else if (rxResult.CommunicationStatus == GattCommunicationStatus.Unreachable)
            {
                throw new NotImplementedException("UNREACHABLE");
            }
            else if (rxResult.CommunicationStatus == GattCommunicationStatus.AccessDenied)
            {
                throw new NotImplementedException("ACCESSDENIED");
            }
            return rxResult;

        }

        public async Task<GattWriteResult> WriteAsync(byte[] value)
        {
            var writer = new DataWriter();
            writer.WriteBytes(value);
            IBuffer buffer = writer.DetachBuffer();
            var result = (await Win10Characteristic.WriteValueWithResultAsync(buffer)).ToRXGattWriteResult();
            return result;
        }

        public async Task<GattCommunicationStatus> WriteWithoutResponseAsync(byte[] value)
        {
            var writer = new DataWriter();
            writer.WriteBytes(value);
            IBuffer buffer = writer.DetachBuffer();
            GattCommunicationStatus status = (await Win10Characteristic.WriteValueAsync(buffer)).ToRXCommunicationStatus();
            return status;
        }


        class RXClientCharacteristicConfiguration : IClientCharacteristicConfiguration
        {
            RXGattClientCharacteristic RXGattClientCharacteristic { get; }
            public RXClientCharacteristicConfiguration(RXGattClientCharacteristic rxGattClientCharacteristic)
            {
                RXGattClientCharacteristic = rxGattClientCharacteristic;
            }

            public async Task<GattWriteResult> SetValueAsync(bool notification, bool indication)
            {
                int value = 0;
                if (notification)
                {
                    value |= (int)(Windows.Devices.Bluetooth.GenericAttributeProfile.GattClientCharacteristicConfigurationDescriptorValue.Notify);
                }
                if (indication)
                {
                    value |= (int)(Windows.Devices.Bluetooth.GenericAttributeProfile.GattClientCharacteristicConfigurationDescriptorValue.Indicate); ;
                }
                var win10Result = await RXGattClientCharacteristic.Win10Characteristic.WriteClientCharacteristicConfigurationDescriptorWithResultAsync((Windows.Devices.Bluetooth.GenericAttributeProfile.GattClientCharacteristicConfigurationDescriptorValue)value);
                var rxResult = win10Result.ToRXGattWriteResult();
                return rxResult;
            }
        }
    }
}
