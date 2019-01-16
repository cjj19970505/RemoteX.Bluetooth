using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Bluetooth.LE.Gatt
{
    /// <summary>
    /// Primary services can be discovered using Primary Service Discovery procedures.
    /// A sencondary service is a service that is only intended to be referenced from a primary service or another secondary service or other higher layer specification
    /// From Core_v5.0.pdf Page2230
    /// </summary>
    public enum GattServiceType { Primary, Secondary}
    
    public struct CharacteristicReadRequest
    {
        public IBluetoothDevice Device;
        public IGattServerCharacteristic TargetCharacteristic; 
        public int RequestId;
        public int Offset;
        public byte[] Value;
    }
    public struct DescriptorReadRequest
    {
        public IBluetoothDevice Device;
        public IGattServerDescriptor TargetDescriptor;
        public int RequestId;
        public int Offset;
        public byte[] Value;
    }
    public struct WriteRequest
    {
        //BluetoothDevice device, int requestId, BluetoothGattCharacteristic characteristic, bool preparedWrite, bool responseNeeded, int offset, byte[] value
        public IBluetoothDevice Device;
        public int RequestId;
        //bool PreparedWrite;
        public bool ResponseNeeded;
        public int Offset;
        public byte[] Value;
    }
    public struct GattPermissions
    {
        public bool Read;
        public bool Write;
    }

    public interface IGattServerService:IGattService
    {
        IGattServerCharacteristic[] Characteristics { get; }
        IGattServer Server { get; }
    }

    public interface IGattServerCharacteristic:IGattCharacteristic
    {
        event EventHandler<CharacteristicReadRequest> OnRead;
        event EventHandler<WriteRequest> OnWrite;
        IGattServerService Service { get; }
        byte[] Value { get; set; }
        void NotifyValueChanged(IBluetoothDevice bluetoothDevice, bool confirm);
        
        //BluetoothDevice device, int requestId, BluetoothGattCharacteristic characteristic, bool preparedWrite, bool responseNeeded, int offset, byte[] value

    }

    public interface IGattServerDescriptor:IGattDescriptor
    {
        event EventHandler<DescriptorReadRequest> OnRead;
        event EventHandler<WriteRequest> OnWrite;
        
    }

    /// <summary>
    /// 3.3.1.1
    /// The Characteristic Properties bit field determines how the Characteristic Value can be used, or how the characteristic descriptors (see Section 3.3.3) can be accessed. If the bits defined in Table 3 5 are set, the action described is permitted. Multiple Characteristic Properties can be set.
    /// </summary>
    public struct GattCharacteristicProperties
    {
        public bool Broadcast;
        public bool Read;
        public bool WriteWithoutResponse;
        public bool Write;
        public bool Notify;
        public bool Indicate;
        public bool AuthenticatedSignedWrites;
        public bool ExtendedProperties;
    }


    
}
