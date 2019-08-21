using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RemoteX.Bluetooth.Rfcomm;

namespace RemoteX.Bluetooth.Droid.Rfcomm
{
    class RfcommConnection : IRfcommConnection
    {
        public IBluetoothDevice RemoteDevice { get; }

        public Android.Bluetooth.BluetoothSocket DroidSocket { get; }
        public Stream InputStream { get; private set; }

        public Stream OutputStream { get; private set; }

        public RfcommConnection(IBluetoothDevice remoteDevice, Android.Bluetooth.BluetoothSocket droidSocket)
        {
            RemoteDevice = remoteDevice;
            DroidSocket = droidSocket;
            InputStream = DroidSocket.InputStream;
            OutputStream = DroidSocket.OutputStream;
            
        }

        public void Dispose()
        {
            InputStream = null;
            OutputStream = null;
            DroidSocket.Dispose();
        }
    }
}