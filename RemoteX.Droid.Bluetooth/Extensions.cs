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
using Java.Util;
using RemoteX.Bluetooth;
using static RemoteX.Bluetooth.Droid.BluetoothManager;

namespace RemoteX.Bluetooth.Droid
{
    static class Extensions
    {
        public static BluetoothDeviceWrapper GetFromAddress(this IEnumerable<BluetoothDeviceWrapper> bluetoothDevices, string macAddress)
        {
            foreach(var device in bluetoothDevices)
            {
                if(BluetoothUtils.AddressStringToInt64(macAddress) == device.Address)
                {
                    return device;
                }
            }
            return null;
        }

        public static Guid ToGuid(this UUID self)
        {
            long firstLong = self.LeastSignificantBits;
            long secondLong = self.MostSignificantBits;
            var firstLongBytes = BitConverter.GetBytes(firstLong);
            List<byte> firstLongInvList = new List<byte>();
            for(int i = firstLongBytes.Length-1;i>=0;i--)
            {
                firstLongInvList.Add(firstLongBytes[i]);
            }
            long firstLongInv = BitConverter.ToInt64(firstLongInvList.ToArray(), 0);

            List<byte> secondLongInvList = new List<byte>();
            var secondLongBytes = BitConverter.GetBytes(secondLong);
            for (int i = secondLongBytes.Length - 1; i >= 0; i--)
            {
                secondLongInvList.Add(secondLongBytes[i]);
            }
            long secondLongInv = BitConverter.ToInt64(secondLongInvList.ToArray(), 0);
            UUID newUUID = new UUID(firstLongInv, secondLongInv);
            return Guid.Parse(newUUID.ToString());
        }
    }
}