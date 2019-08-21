using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.Procedure.Server
{
    public class RfcommServerServiceWrapper
    {
        public IGattServerService GattServerService { get; private set; }
        public IBluetoothManager BluetoothManager { get; }
        public RfcommServerAddressCharacteristicWrapper RfcommServerAddressCharacteristicWrapper { get; }

        public RfcommServerServiceWrapper(IBluetoothManager bluetoothManager)
        {
            BluetoothManager = bluetoothManager;
            RfcommServerAddressCharacteristicWrapper = new RfcommServerAddressCharacteristicWrapper(this);
            GattServerService = BluetoothManager.NewGattServiceBuilder()
                .SetUuid(Constants.RfcommServerServiceGuid)
                .AddCharacteristics(RfcommServerAddressCharacteristicWrapper.GattServerCharacteristic)
                .Build();
        }
    }

    public class RfcommServerAddressCharacteristicWrapper
    {
        public IGattServerCharacteristic GattServerCharacteristic { get; private set; }
        public ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper { get; }
        public RfcommServerServiceWrapper RfcommServerServiceWrapper { get; }

        private static GattCharacteristicProperties Properties = new GattCharacteristicProperties
        {
            Read = true,
        };

        private static GattPermissions Permission = new GattPermissions
        {
            Read = true,
        };

        public RfcommServerAddressCharacteristicWrapper(RfcommServerServiceWrapper serviceWrapper)
        {
            RfcommServerServiceWrapper = serviceWrapper;
            ClientCharacteristicConfigurationDescriptorWrapper = new ClientCharacteristicConfigurationDescriptorWrapper(serviceWrapper.BluetoothManager);
            var characteristic = RfcommServerServiceWrapper.BluetoothManager.NewGattCharacteristicBuilder()
                    .SetUuid(Constants.RfcommServerAddressCharacteristicGuid)
                    .AddDescriptors(ClientCharacteristicConfigurationDescriptorWrapper.GattServerDescriptor)
                    .SetPermissions(Permission)
                    .SetProperties(Properties)
                    .Build();
            GattServerCharacteristic = characteristic;
            GattServerCharacteristic.OnRead += GattServerCharacteristic_OnRead;
        }

        private void GattServerCharacteristic_OnRead(object sender, ICharacteristicReadRequest e)
        {
            e.RespondWithValue(BitConverter.GetBytes(RfcommServerServiceWrapper.BluetoothManager.MacAddress));
        }
    }
}
