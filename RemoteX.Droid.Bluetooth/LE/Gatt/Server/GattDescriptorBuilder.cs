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

namespace RemoteX.Droid.Bluetooth.LE.Gatt.Server
{
    class GattDescriptorBuilder : IGattDescriptorBuilder
    {
        public Guid Uuid { get; set; }
        public GattPermissions Permissions { get; set; }

        public IGattServerDescriptor Build()
        {
            return new GattServer.GattServerService.GattServerCharacteristic.GattServerDescriptor(Uuid, Permissions);
        }

        public IGattDescriptorBuilder SetPermissions(GattPermissions permissions)
        {
            Permissions = permissions;
            return this;
        }

        public IGattDescriptorBuilder SetUuid(Guid uuid)
        {
            Uuid = uuid;
            return this;
        }
    }
}