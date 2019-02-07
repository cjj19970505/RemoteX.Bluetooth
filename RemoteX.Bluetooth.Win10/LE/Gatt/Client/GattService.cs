using RemoteX.Bluetooth.LE.Att;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Bluetooth.Win10.LE.Gatt.Client
{
    internal class RXGattClientService : IGattClientService, IAttribute
    {
        public GattDeviceService Win10GattService { get; }

        public ushort AttributeHandle
        {
            get
            {
                return Win10GattService.AttributeHandle;
            }
        }

        public Guid Uuid
        {
            get
            {
                return Win10GattService.Uuid;
            }
        }

        public GattServiceType ServiceType => throw new NotImplementedException();

        public RXGattClientService(GattDeviceService win10GattService)
        {
            Win10GattService = win10GattService;
        }

        public Task<IGattClientCharacteristic[]> DiscoverAllCharacteristicsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
