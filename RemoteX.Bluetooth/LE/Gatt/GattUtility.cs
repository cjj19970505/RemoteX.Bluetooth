using RemoteX.Bluetooth.LE.Gatt.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt
{
    public static class GattUtility
    {
        public static string ToGattServiceName(this Guid self)
        {
            if (self == BatteryServiceWrapper.BATTERY_SERVICE_UUID)
            {
                return "Battery Service";
            }
            return "Unknown Service";
        }
        
    }
}
