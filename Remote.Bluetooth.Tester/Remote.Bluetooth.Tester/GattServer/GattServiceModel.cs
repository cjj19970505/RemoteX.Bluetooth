using RemoteX.Bluetooth.LE.Gatt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Remote.Bluetooth.Tester.GattServer
{
    public class GattServiceModel
    {
        public IGattServerService GattServerService { get; }
        public Guid Uuid
        {
            get
            {
                return GattServerService.Uuid;
            }
        }
        public string Name
        {
            get
            {
                return GattServerService.Uuid.ToGattServiceName();
            }
        }
        
        public GattServiceModel(IGattServerService gattServerService)
        {
            GattServerService = gattServerService;
        }

    }
}
