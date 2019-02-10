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
    }
}