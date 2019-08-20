using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.Procedure.Server
{
    public class ClientRfcommServiceWrapper
    {

        public IGattServerService GattServerService { get; private set; }
        public IBluetoothManager BluetoothManager { get; }
        public ClientRfcommAddressCharacteristicWrapper AddressCharacteristicWrapper { get; }
        public event EventHandler<KeyValuePair<IBluetoothDevice, UInt64>> OnRfcommAddressWrite;
        public ClientRfcommServiceWrapper(IBluetoothManager bluetoothManager)
        {
            BluetoothManager = bluetoothManager;
            AddressCharacteristicWrapper = new ClientRfcommAddressCharacteristicWrapper(this);
            GattServerService = BluetoothManager.NewGattServiceBuilder()
                .SetUuid(Constants.ClientRfcommServiceGuid)
                .AddCharacteristics(AddressCharacteristicWrapper.GattServerCharacteristic)
                .Build();
        }

        public class ClientRfcommAddressCharacteristicWrapper
        {

            public Dictionary<IBluetoothDevice, UInt64> RfcommAddressDict;
            public IGattServerCharacteristic GattServerCharacteristic { get; private set; }
            public ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper { get; }
            public ClientRfcommServiceWrapper ClientRfcommServiceWrapper { get; }

            private static GattCharacteristicProperties Properties = new GattCharacteristicProperties
            {
                Read = true,
                Write = true,

            };

            private static GattPermissions Permission = new GattPermissions
            {
                Read = true,
                Write = true
                
            };

            public ClientRfcommAddressCharacteristicWrapper(ClientRfcommServiceWrapper serviceWrapper)
            {
                RfcommAddressDict = new Dictionary<IBluetoothDevice, ulong>();
                ClientCharacteristicConfigurationDescriptorWrapper = new ClientCharacteristicConfigurationDescriptorWrapper(serviceWrapper.BluetoothManager);
                ClientRfcommServiceWrapper = serviceWrapper;
                var characteristic = ClientRfcommServiceWrapper.BluetoothManager.NewGattCharacteristicBuilder()
                    .SetUuid(Constants.ClientRfcommAddressCharacteristicGuid)
                    .AddDescriptors(ClientCharacteristicConfigurationDescriptorWrapper.GattServerDescriptor)
                    .SetPermissions(Permission)
                    .SetProperties(Properties)
                    .Build();
                GattServerCharacteristic = characteristic;
                GattServerCharacteristic.OnRead += GattServerCharacteristic_OnRead;
                GattServerCharacteristic.OnWrite += GattServerCharacteristic_OnWrite;
            }

            private void GattServerCharacteristic_OnWrite(object sender, ICharacteristicWriteRequest e)
            {
                ulong address;
                try
                {
                    address = BitConverter.ToUInt64(e.Value, 0);
                }
                catch (Exception)
                {
                    e.RespondWithProtocolError(GattErrorCode.Failure);
                    return;
                }
                if (!RfcommAddressDict.ContainsKey(e.SourceDevice))
                {
                    RfcommAddressDict.Add(e.SourceDevice, address);
                }
                else
                {
                    RfcommAddressDict[e.SourceDevice] = address;
                }
                e.RespondSuccess();
                ClientRfcommServiceWrapper.OnRfcommAddressWrite?.Invoke(ClientRfcommServiceWrapper, new KeyValuePair<IBluetoothDevice, ulong>(e.SourceDevice, address));
            }

            private void GattServerCharacteristic_OnRead(object sender, ICharacteristicReadRequest e)
            {
                if(RfcommAddressDict.ContainsKey(e.SourceDevice))
                {
                    e.RespondWithValue(BitConverter.GetBytes(RfcommAddressDict[e.SourceDevice]));
                }
                else
                {
                    e.RespondWithProtocolError(GattErrorCode.Failure);
                }
            }
        }
    }
}
