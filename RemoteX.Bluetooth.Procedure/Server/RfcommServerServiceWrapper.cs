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
        public RfcommServerAddressCharacteristicWrapper AddressCharacteristicWrapper { get; }
        public RfcommServerNameCharacteristicWrapper NameCharacteristicWrapper { get; }
        public RfcommServerServiceWrapper(IBluetoothManager bluetoothManager)
        {
            BluetoothManager = bluetoothManager;
            AddressCharacteristicWrapper = new RfcommServerAddressCharacteristicWrapper(this);
            NameCharacteristicWrapper = new RfcommServerNameCharacteristicWrapper(this);
            GattServerService = BluetoothManager.NewGattServiceBuilder()
                .SetUuid(Constants.RfcommServerServiceGuid)
                .AddCharacteristics(AddressCharacteristicWrapper.GattServerCharacteristic)
                .AddCharacteristics(NameCharacteristicWrapper.GattServerCharacteristic)
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
            var bytes = BitConverter.GetBytes(RfcommServerServiceWrapper.BluetoothManager.MacAddress);
            e.RespondWithValue(bytes);
        }
    }

    public class RfcommServerNameCharacteristicWrapper
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

        public RfcommServerNameCharacteristicWrapper(RfcommServerServiceWrapper serviceWrapper)
        {
            RfcommServerServiceWrapper = serviceWrapper;
            ClientCharacteristicConfigurationDescriptorWrapper = new ClientCharacteristicConfigurationDescriptorWrapper(serviceWrapper.BluetoothManager);
            var characteristic = RfcommServerServiceWrapper.BluetoothManager.NewGattCharacteristicBuilder()
                    .SetUuid(Constants.RfcommServerNameCharacteristicGuid)
                    .AddDescriptors(ClientCharacteristicConfigurationDescriptorWrapper.GattServerDescriptor)
                    .SetPermissions(Permission)
                    .SetProperties(Properties)
                    .Build();
            GattServerCharacteristic = characteristic;
            GattServerCharacteristic.OnRead += GattServerCharacteristic_OnRead; ;
        }

        private void GattServerCharacteristic_OnRead(object sender, ICharacteristicReadRequest e)
        {
            e.RespondWithValue(Encoding.UTF8.GetBytes(RfcommServerServiceWrapper.BluetoothManager.Name));
        }
    }
}
