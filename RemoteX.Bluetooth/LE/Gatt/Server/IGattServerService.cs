using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Bluetooth.LE.Gatt.Server
{
    /// <summary>
    /// Primary services can be discovered using Primary Service Discovery procedures.
    /// A sencondary service is a service that is only intended to be referenced from a primary service or another secondary service or other higher layer specification
    /// From Core_v5.0.pdf Page2230
    /// </summary>
    

    public interface IGattServerService:IGattService
    {
        IGattServerCharacteristic[] Characteristics { get; }
        IGattServer Server { get; }
    }

    public interface IGattServerCharacteristic:IGattCharacteristic
    {
        event EventHandler<ICharacteristicReadRequest> OnRead;
        event EventHandler<ICharacteristicWriteRequest> OnWrite;
        IGattServerService Service { get; }
        byte[] Value { get; set; }
        void NotifyValueChanged(IBluetoothDevice bluetoothDevice, bool confirm);
        
        //BluetoothDevice device, int requestId, BluetoothGattCharacteristic characteristic, bool preparedWrite, bool responseNeeded, int offset, byte[] value

    }

    public interface IGattServerDescriptor:IGattDescriptor
    {
        event EventHandler<IDescriptorReadRequest> OnRead;
        event EventHandler<IDescriptorWriteRequest> OnWrite;
        
    }

    


    
}
