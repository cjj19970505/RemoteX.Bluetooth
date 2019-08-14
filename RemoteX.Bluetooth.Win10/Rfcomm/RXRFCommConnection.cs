using RemoteX.Bluetooth.Rfcomm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace RemoteX.Bluetooth.Win10.Rfcomm
{
    internal class RXRFCommConnection : IRfcommConnection
    {
        public IRfcommServiceProvider ServiceProvider { get; }
        public IRfcommDeviceService RemoteDeviceService { get; }
        public IBluetoothDevice RemoteDevice { get; }

        public StreamSocket StreamSocket { get; private set; }

        public Stream InputStream { get; private set; }

        public Stream OutputStream { get; private set; }

        internal event EventHandler Disposed;

        public RXRFCommConnection(RXRfcommServiceProvider provider, IBluetoothDevice remoteDevice, StreamSocket streamSocket)
        {
            ServiceProvider = provider;
            RemoteDevice = remoteDevice;
            StreamSocket = streamSocket;

            InputStream = streamSocket.InputStream.AsStreamForRead();
            OutputStream = streamSocket.OutputStream.AsStreamForWrite();

        }
        public RXRFCommConnection(IRfcommDeviceService remoteDeviceService, IBluetoothDevice remoteDevice, StreamSocket streamSocket)
        {
            RemoteDeviceService = remoteDeviceService;
            RemoteDevice = remoteDevice;
            StreamSocket = streamSocket;

            InputStream = streamSocket.InputStream.AsStreamForRead();
            OutputStream = streamSocket.OutputStream.AsStreamForWrite();

        }

        public void Dispose()
        {
            InputStream = null;
            OutputStream = null;
            StreamSocket.Dispose();
            StreamSocket = null;
            Disposed?.Invoke(this, null);
        }
    }
}
