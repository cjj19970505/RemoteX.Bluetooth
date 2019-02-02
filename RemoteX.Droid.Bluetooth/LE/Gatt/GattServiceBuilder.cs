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
using static RemoteX.Droid.Bluetooth.LE.Gatt.GattServer;
using static RemoteX.Droid.Bluetooth.LE.Gatt.GattServer.GattServerService;

namespace RemoteX.Droid.Bluetooth.LE.Gatt
{
    class GattServiceBuilder : IGattServiceBuilder
    {
        public GattServiceType ServiceType { get; set; }
        public Guid Uuid { get; set; }
        private List<IGattServerCharacteristic> _CharacteristicsList;
        public GattServiceBuilder()
        {
            _CharacteristicsList = new List<IGattServerCharacteristic>();
        }

        public IGattServiceBuilder AddCharacteristics(params IGattServerCharacteristic[] characteristics)
        {
            AddCharacteristics(characteristics as IEnumerable<IGattServerCharacteristic>);
            return this;
        }

        public IGattServiceBuilder AddCharacteristics(IEnumerable<IGattServerCharacteristic> characteristics)
        {
            _CharacteristicsList.AddRange(characteristics);
            return this;
        }

        public IGattServerService Build()
        {
            GattServerService gattServerService = new GattServerService(Uuid);
            
            foreach (var characteristic in _CharacteristicsList)
            {
                gattServerService.AddCharacteristic(characteristic as GattServerCharacteristic);
            }
            return gattServerService;

        }

        public IGattServiceBuilder SetServiceType(GattServiceType gattServiceType)
        {
            ServiceType = gattServiceType;
            return this;
        }

        public IGattServiceBuilder SetUuid(Guid uuid)
        {
            Uuid = uuid;
            return this;
        }
    }
}