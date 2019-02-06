using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteX.Bluetooth.LE.Gatt.Client;
using RemoteX.Bluetooth.Win10.LE.Gatt.Client;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace RemoteX.Bluetooth.Win10.LE
{
    class RXBluetoothDevice:IBluetoothDevice
    {
        public BluetoothLEDevice Win10LEDevice { get; }
        public DeviceInformation Win10DeviceInformaion { get; }
        public BluetoothManager RXBluetoothManager { get; }
        public IGattClient GattClient { get; }
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
                return BluetoothUtils.AddressStringToInt64(RXBluetoothUtils.GetAddressStringFromDeviceId(Win10DeviceInformaion.Id));
            }
        }

        public RXBluetoothDevice(BluetoothManager bluetoothManager ,DeviceInformation deviceInformation)
        {
            RXBluetoothManager = bluetoothManager;
            Win10DeviceInformaion = deviceInformation;
            GattClient = new RXGattClient(this);
        }

        
    }
}
