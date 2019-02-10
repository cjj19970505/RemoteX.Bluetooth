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
using RemoteX.Bluetooth.Droid.LE;

namespace RemoteX.Bluetooth.Droid.LE.Gatt.Server
{
    public partial class GattServer: IGattServer
    {
        public partial class GattServerService : IGattServerService
        {
            public Android.Bluetooth.BluetoothGattService DroidService { get; private set; }

            public GattServiceType ServiceType
            {
                get
                {
                    if(DroidService.Type == Android.Bluetooth.GattServiceType.Primary)
                    {
                        return GattServiceType.Primary;
                    }
                    return GattServiceType.Secondary;
                }
            }



            public Guid Uuid
            {
                get
                {
                    return DroidService.Uuid.ToGuid();
                }
            }

            /// <summary>
            /// Only avalible when the service is added to the server
            /// </summary>
            public IGattServer Server { get; private set; }

            public List<GattServerCharacteristic> GattCharacteristics { get; private set; }

            public IGattServerCharacteristic[] Characteristics
            {
                get
                {
                    return GattCharacteristics.ToArray();
                }
            }

            public GattServerService(Guid uuid)
            {
                GattCharacteristics = new List<GattServerCharacteristic>();
                DroidService = new Android.Bluetooth.BluetoothGattService(Java.Util.UUID.FromString(uuid.ToString()), Android.Bluetooth.GattServiceType.Primary);
            }

            public void AddCharacteristic(GattServerCharacteristic characteristic)
            {
                characteristic.AddToService(this);
                //DroidService.AddCharacteristic(characteristic.DroidCharacteristic);
            }

            public void AddToServer(GattServer server)
            {
                this.Server = server;
                server._GattServices.Add(this);
                server.DroidGattServer.AddService(DroidService);
            }
        }
    }
    
}