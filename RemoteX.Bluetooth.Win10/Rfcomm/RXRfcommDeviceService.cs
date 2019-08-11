﻿using RemoteX.Bluetooth.Rfcomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using System.IO;
using System.Net.Sockets;

namespace RemoteX.Bluetooth.Win10.Rfcomm
{
    internal class RXRfcommDeviceService : IRfcommDeviceService
    {
        public RfcommDeviceService Win10RfcommDeviceService { get; }
        public Guid ServiceId
        {
            get
            {
                return Win10RfcommDeviceService.ServiceId.Uuid;
            }
        }
        public IBluetoothDevice Device { get; }

        public StreamSocket Socket { get; }
        public Stream InputStream { get; private set; }
        public Stream OutputStream { get; private set; }

        public RXRfcommDeviceService(IBluetoothDevice bluetoothDevice, RfcommDeviceService win10Service)
        {
            Socket = new StreamSocket();
            Device = bluetoothDevice;
            Win10RfcommDeviceService = win10Service;
        }

        public async Task ConnectAsync()
        {
            await Socket.ConnectAsync(Win10RfcommDeviceService.ConnectionHostName, Win10RfcommDeviceService.ConnectionServiceName);
            InputStream = Socket.InputStream.AsStreamForRead();
            OutputStream = Socket.OutputStream.AsStreamForWrite();
        }

    }
}
