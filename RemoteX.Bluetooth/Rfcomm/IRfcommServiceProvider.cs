using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Bluetooth.Rfcomm
{
    public interface IRfcommServiceProvider
    {
        Task<IRfcommDeviceService> CreateAsync(Guid serviceId);
        
    }
}
