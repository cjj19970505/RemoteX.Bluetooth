using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;
using static RemoteX.Droid.Bluetooth.LE.Gatt.GattServer.GattServerService;
using static RemoteX.Droid.Bluetooth.LE.Gatt.GattServer.GattServerService.GattServerCharacteristic;
using static RemoteX.Droid.Bluetooth.LE.Gatt.GattServer.GattServerService.GattServerCharacteristic.GattServerDescriptor;

namespace RemoteX.Droid.Bluetooth.LE.Gatt
{
    class GattCharacteristicBuilder : IGattCharacteristicBuilder
    {
        private List<IGattServerDescriptor> _DescriptorsList;

        public GattCharacteristicBuilder()
        {
            _DescriptorsList = new List<IGattServerDescriptor>();
        }

        public GattCharacteristicProperties Properties { get; set; }
        public Guid Uuid { get; set; }

        public GattPermissions Permissions { get; private set; }

        public IGattCharacteristicBuilder AddDescriptors(params IGattServerDescriptor[] gattDescriptors)
        {
            AddDescriptors(gattDescriptors as IEnumerable<IGattServerDescriptor>);
            return this;
        }

        public IGattCharacteristicBuilder AddDescriptors(IEnumerable<IGattServerDescriptor> gattDescriptors)
        {
            _DescriptorsList.AddRange(gattDescriptors);
            return this;
        }

        public IGattServerCharacteristic Build()
        {
            GattServerCharacteristic characteristic = new GattServerCharacteristic(Uuid, Properties, Permissions);
            foreach(var descriptor in _DescriptorsList)
            {
                characteristic.AddDescriptor(descriptor as GattServerDescriptor);
            }
            return characteristic;
        }

        public IGattCharacteristicBuilder SetPermissions(GattPermissions permissions)
        {
            Permissions = permissions;
            return this;
        }

        public IGattCharacteristicBuilder SetProperties(GattCharacteristicProperties properties)
        {
            Properties = properties;
            return this;
        }

        public IGattCharacteristicBuilder SetUuid(Guid uuid)
        {
            Uuid = uuid;
            return this;
        }
    }
}