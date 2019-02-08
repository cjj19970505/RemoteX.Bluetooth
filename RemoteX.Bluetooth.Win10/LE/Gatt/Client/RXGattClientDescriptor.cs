using RemoteX.Bluetooth.LE.Gatt.Client;
using RemoteX.Bluetooth.LE.Gatt.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace RemoteX.Bluetooth.Win10.LE.Gatt.Client
{
    internal class RXGattClientDescriptor : IGattClientDescriptor
    {
        public IGattServerCharacteristic Characteristic => throw new NotImplementedException();

        public Guid Uuid
        {
            get
            {
                return Win10Descriptor.Uuid;
            }
        }

        public ushort AttributeHandle
        {
            get
            {
                return Win10Descriptor.AttributeHandle;
            }
        }

        public GattDescriptor Win10Descriptor { get; }

        public RXGattClientDescriptor(GattDescriptor win10Descriptor)
        {
            Win10Descriptor = win10Descriptor;
        }

        public async Task<Bluetooth.LE.Gatt.Client.GattWriteResult> WriteAsync(byte[] value)
        {
            var writer = new DataWriter();
            //writer.WriteBytes(value);
            writer.WriteByte(0x01);
            
            System.Diagnostics.Debug.WriteLine("START WRITE");
            var result = await Win10Descriptor.WriteValueAsync(writer.DetachBuffer());
            System.Diagnostics.Debug.WriteLine("RESULT::" + Enum.GetName(typeof(GattCommunicationStatus), result));
            return new Bluetooth.LE.Gatt.Client.GattWriteResult();
            //return (await Win10Descriptor.WriteValueWithResultAsync(writer.DetachBuffer())).ToRXGattWriteResult();
        }
    }
}
