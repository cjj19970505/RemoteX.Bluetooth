using RemoteX.Bluetooth.Rfcomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using System.IO;
using System.Net.Sockets;

namespace RemoteX.Bluetooth.Win10.Rfcomm
{
    public class RXRfcommServiceProvider : IRfcommServiceProvider
    {
        
        public Task<IRfcommDeviceService> CreateAsync(Guid serviceId)
        {
        }
    }
}
