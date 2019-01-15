using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    
    public interface IGattCharacteristic
    {
        IGattServerDescriptor[] Descriptors { get; }
        
        GattPermissions Permissions { get; }

        //(Service.Server as GattServer).DroidGattServer.SendResponse(device, requestId, GattStatus.Success, offset, new byte[] { BitConverter.GetBytes(BatteryLevel)[0]});
        //public virtual void OnCharacteristicRead(Android.Bluetooth.BluetoothDevice device, int requestId, int offset)
        

        //3.3.1 Characteristic declaration
        //Attribute Value Field
        /// <summary>
        /// 3.3.1.1
        /// The Characteristic Properties bit field determines how the Characteristic Value can be used, or how the characteristic descriptors (see Section 3.3.3) can be accessed. If the bits defined in Table 3 5 are set, the action described is permitted. Multiple Characteristic Properties can be set.
        /// </summary>
        GattCharacteristicProperties CharacteristicProperties { get; }
        /// <summary>
        /// 3.3.1.2
        /// The Characteristic Value Attribute Handle field is the Attribute Handle of the Attribute that contains the Characteristic Value.
        /// </summary>
        int CharacteristicValueHandle { get; }
        /// <summary>
        /// 3.3.1.3
        /// The Characteristic UUID field is a 16-bit Bluetooth UUID or 128-bit UUID that describes the type of Characteristic Value
        /// </summary>
        Guid Uuid { get; }

        //3.3.2 Characteristic Value Declaration
        //byte[] Value { get; }
    }
}
