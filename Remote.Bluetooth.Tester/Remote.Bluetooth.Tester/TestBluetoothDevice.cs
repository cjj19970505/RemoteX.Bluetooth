using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Remote.Bluetooth.Tester
{

    public class TestBluetoothDevice : IBluetoothDevice
    {
        public string Name { get; set; }

        public ulong Address { get; set; }

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

        public TestBluetoothDevice(string name, ulong address)
        {
            Name = name;
            Address = address;
        }
    }
}