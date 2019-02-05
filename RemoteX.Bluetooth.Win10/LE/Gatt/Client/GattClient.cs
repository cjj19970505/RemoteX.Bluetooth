using RemoteX.Bluetooth.LE.Gatt.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;

namespace RemoteX.Bluetooth.Win10.LE.Gatt.Client
{
    internal class GattClient:IGattClient
    {
        
        public RXBluetoothDevice BluetoothDevice { get; }
        public BluetoothLEDevice Win10LEDevice { get; private set; }
        public GattClient(RXBluetoothDevice bluetoothDevice)
        {
            BluetoothDevice = bluetoothDevice;
        }

        public event EventHandler<IGattClientService[]> OnServicesDiscovered;

        public async Task ConnectToServerAsync()
        {
            Win10LEDevice = await BluetoothLEDevice.FromIdAsync(BluetoothDevice.Win10DeviceInformaion.Id);
            System.Diagnostics.Debug.WriteLine(Win10LEDevice.DeviceId);
        }

        

        public Task<IGattClientService[]> DiscoverAllPrimaryServiceAsync()
        {
            throw new NotImplementedException();
        }
    }
}
