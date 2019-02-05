using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteX.Bluetooth.LE.Gatt.Client;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace RemoteX.Bluetooth.Win10.LE
{
    class RXBluetoothDevice:IBluetoothDevice
    {
        public BluetoothLEDevice Win10LEDevice { get; }
        public DeviceInformation Win10DeviceInformaion { get; }

        public string Name
        {
            get
            {
                return Win10DeviceInformaion.Name;
            }
        }

        public ulong Address
        {
            get
            {
                return Win10LEDevice.BluetoothAddress;
            }
        }

        public Guid[] LastestFetchedUuids => throw new NotImplementedException();

        public bool IsFetchingUuids => throw new NotImplementedException();

        public IGattClient GattClient => throw new NotImplementedException();

        public event BluetoothDeviceGetUuidsHanlder OnUuidsFetched;

        public void FetchUuidsWithSdp()
        {
            throw new NotImplementedException();
        }

        public void stopFetchingUuidsWithSdp()
        {
            throw new NotImplementedException();
        }
    }
}
