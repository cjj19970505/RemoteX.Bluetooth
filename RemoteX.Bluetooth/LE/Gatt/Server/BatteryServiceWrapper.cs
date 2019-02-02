﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt.Server
{
    public class BatteryServiceWrapper
    {
        public static Guid BATTERY_SERVICE_UUID = BluetoothUtils.ShortValueUuid(0x180F);
        public IGattServerService GattServerService { get; private set; }

        public BatteryLevelCharacteristicWrapper BatteryLevelCharacteristicWrapper { get; private set; }


        public BatteryServiceWrapper(IBluetoothManager bluetoothManager)
        {
            IGattServiceBuilder builder = bluetoothManager.NewGattServiceBuilder();
            builder.SetUuid(BATTERY_SERVICE_UUID).SetServiceType(GattServiceType.Primary);
            BatteryLevelCharacteristicWrapper = new BatteryLevelCharacteristicWrapper(bluetoothManager);
            builder.AddCharacteristics(BatteryLevelCharacteristicWrapper.GattServerCharacteristic);
            GattServerService = builder.Build();
        }
    }

    public class BatteryLevelCharacteristicWrapper
    {
        public static Guid BATTERY_LEVEL_UUID = BluetoothUtils.ShortValueUuid(0x2A19);
        private static GattCharacteristicProperties PROPERTIES = new GattCharacteristicProperties
        {
            Read = true,
            Notify = true,
        };
        private static GattPermissions PERMISSIONS = new GattPermissions
        {
            Read = true,
        };

        public IGattServerCharacteristic GattServerCharacteristic { get; private set; }

        public ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper { get; private set; }

        private int _BatteryLevel;
        public int BatteryLevel
        {
            get
            {
                return _BatteryLevel;
            }
            set
            {
                if(value != _BatteryLevel)
                {
                    _BatteryLevel = value;
                    GattServerCharacteristic.Value = new byte[] { BitConverter.GetBytes(BatteryLevel)[0] };
                    NotifyAll();
                }
            }
        }

        public void NotifyAll()
        {
            var clientConfigurations = ClientCharacteristicConfigurationDescriptorWrapper.ClientConfigurations;
            System.Diagnostics.Debug.WriteLine("XJ::READY TO NOTIFY TO:" + clientConfigurations.Count);
            foreach(var pair in clientConfigurations)
            {
                System.Diagnostics.Debug.WriteLine("XJ:::("+pair.Key.Name+", "+pair.Value.ToString()+")");
                if (pair.Value.Notifications)
                {
                    GattServerCharacteristic.NotifyValueChanged(pair.Key, false);
                    System.Diagnostics.Debug.WriteLine("XJ:::Notified");
                    
                }
            }
        }

        public BatteryLevelCharacteristicWrapper(IBluetoothManager bluetoothManager)
        {
            ClientCharacteristicConfigurationDescriptorWrapper = new ClientCharacteristicConfigurationDescriptorWrapper(bluetoothManager);

            IGattCharacteristicBuilder builder = bluetoothManager.NewGattCharacteristicBuilder();
            builder.SetUuid(BATTERY_LEVEL_UUID).SetPermissions(PERMISSIONS).SetProperties(PROPERTIES);
            builder.AddDescriptors(ClientCharacteristicConfigurationDescriptorWrapper.GattServerDescriptor);
            GattServerCharacteristic = builder.Build();
            
            GattServerCharacteristic.OnRead += _OnRead;
            BatteryLevel = 10;
        }

        private void _OnRead(object sender, ICharacteristicReadRequest e)
        {
            var device = e.SourceDevice;
            
            e.RespondWithValue(GattServerCharacteristic.Value);
        }
    }
}