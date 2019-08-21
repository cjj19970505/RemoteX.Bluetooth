using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.Procedure
{
    internal static class Constants
    {
        public readonly static Guid ClientRfcommServiceGuid = BluetoothUtils.ShortValueUuid(0x1111);
        public readonly static Guid ClientRfcommAddressCharacteristicGuid = BluetoothUtils.ShortValueUuid(0x1112);
        public readonly static Guid RfcommServerServiceGuid = BluetoothUtils.ShortValueUuid(0x1113);
        public readonly static Guid RfcommServerAddressCharacteristicGuid = BluetoothUtils.ShortValueUuid(0x1114);
        public readonly static Guid RfcommServerNameCharacteristicGuid = BluetoothUtils.ShortValueUuid(0x1115);
    }
}
