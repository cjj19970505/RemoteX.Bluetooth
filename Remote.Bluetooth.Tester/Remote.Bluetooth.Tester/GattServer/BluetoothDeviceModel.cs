using RemoteX.Bluetooth;

namespace Remote.Bluetooth.Tester.GattServer
{
    internal class BluetoothDeviceModel
    {
        public IBluetoothDevice Device { get; }
        public string Name
        {
            get
            {
                return Device.Name;
            }
        }
        public ulong Address
        {
            get
            {
                return Device.Address;
            }
        }
        public BluetoothDeviceModel(IBluetoothDevice device)
        {
            Device = device;
        }
     
    }
}