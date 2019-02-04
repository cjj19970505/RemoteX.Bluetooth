using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteX.Bluetooth.LE.Gatt.Server;

namespace RemoteX.Bluetooth.Win10
{
    class BluetoothManager : IBluetoothManager
    {
        public bool Supported => throw new NotImplementedException();

        public IBluetoothDevice[] PairedDevices => throw new NotImplementedException();

        public IGattServer GattSever => throw new NotImplementedException();

        public event BluetoothScanResultHandler OnDevicesFound;
        public event BluetoothStartEndScanHandler OnDiscoveryFinished;
        public event BluetoothStartEndScanHandler OnDiscoveryStarted;

        public IBluetoothDevice GetBluetoothDevice(ulong macAddress)
        {
            throw new NotImplementedException();
        }

        public IGattCharacteristicBuilder NewGattCharacteristicBuilder()
        {
            throw new NotImplementedException();
        }

        public IGattDescriptorBuilder NewGattDescriptorBuilder()
        {
            throw new NotImplementedException();
        }

        public IGattServiceBuilder NewGattServiceBuilder()
        {
            throw new NotImplementedException();
        }

        public void SearchForBlutoothDevices()
        {
            throw new NotImplementedException();
        }
    }
}
