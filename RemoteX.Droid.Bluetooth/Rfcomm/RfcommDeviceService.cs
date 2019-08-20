using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using RemoteX.Bluetooth.Rfcomm;
using static RemoteX.Bluetooth.Droid.BluetoothManager;

namespace RemoteX.Bluetooth.Droid.Rfcomm
{
    public class RfcommDeviceService : IRfcommDeviceService
    {
        public Guid ServiceId { get; }

        public IBluetoothDevice Device { get; }

        public IRfcommConnection RfcommConnection { get; private set; }

        public Android.Bluetooth.BluetoothDevice DroidDevice
        {
            get
            {
                return (Device as BluetoothDeviceWrapper).DroidDevice;
            }
        }

        public RfcommDeviceService(IBluetoothDevice device, Guid serviceId)
        {
            ServiceId = serviceId;
            Device = device;
        }

        public Task ConnectAsync()
        {
            return Task.Run(() =>
            {
                Android.Bluetooth.BluetoothSocket bluetoothSocket;
                bluetoothSocket = DroidDevice.CreateInsecureRfcommSocketToServiceRecord(UUID.FromString(ServiceId.ToString()));
                //bluetoothSocket = DroidDevice.CreateInsecureRfcommSocketToServiceRecord(UUID.FromString("4fb996ea-01dc-466c-8b95-9a018c289cef"));
                bluetoothSocket.Connect();
                RfcommConnection = new RfcommConnection(Device, bluetoothSocket);
            });
        }


    }
}