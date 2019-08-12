using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteX.Bluetooth.LE;
using RemoteX.Bluetooth.LE.Gatt.Server;
using RemoteX.Bluetooth.Win10.LE;
using Windows.Devices.Enumeration;
using RemoteX.Bluetooth.Rfcomm;
using RemoteX.Bluetooth.Win10.Rfcomm;
using Windows.Devices.Bluetooth.Rfcomm;

namespace RemoteX.Bluetooth.Win10
{
    public class BluetoothManager : IBluetoothManager
    {
        public bool Supported => throw new NotImplementedException();

        public IBluetoothDevice[] PairedDevices => throw new NotImplementedException();

        public IGattServer GattSever => throw new NotImplementedException();

        /// <summary>
        /// 储存着所有找到的Device
        /// </summary>
        private List<RXBluetoothDevice> _BluetoothDeviceList;

        internal RXBluetoothDevice GetBluetoothDeviceFromDeviceInformation(DeviceInformation deviceInformation)
        {
            ulong address = BluetoothUtils.AddressStringToInt64(RXBluetoothUtils.GetAddressStringFromDeviceId(deviceInformation.Id));
            foreach(var device in _BluetoothDeviceList)
            {
                if(device.Address == address)
                {
                    return device;
                }
            }
            RXBluetoothDevice rxBluetoothDevice = new RXBluetoothDevice(this, deviceInformation);
            _BluetoothDeviceList.Add(rxBluetoothDevice);
            return rxBluetoothDevice;
        }

        internal RXBluetoothDevice RemoveBluetoothDeviceFromDeviceInformationUpdate(DeviceInformationUpdate deviceInformationUpdate)
        {
            ulong address = BluetoothUtils.AddressStringToInt64(RXBluetoothUtils.GetAddressStringFromDeviceId(deviceInformationUpdate.Id));
            RXBluetoothDevice deviceReadyToDelete = null;
            foreach(var device in _BluetoothDeviceList)
            {
                if(device.Address == address)
                {
                    deviceReadyToDelete = device;
                    break;
                }
            }
            if(deviceReadyToDelete != null)
            {
                _BluetoothDeviceList.Remove(deviceReadyToDelete);
            }
            return deviceReadyToDelete;
        }

        public IBluetoothLEScanner LEScanner { get; }
        public IBluetoothRfcommScanner RfcommScanner { get; }

        public Windows.UI.Core.CoreDispatcher Dispatcher { get; }

        private List<RXRfcommServiceProvider> _ServiceProviderList;

        public IRfcommServiceProvider[] ServiceProviders
        {
            get
            {
                return (from provider in _ServiceProviderList
                        select provider as IRfcommServiceProvider).ToArray();
            }
        }

        public BluetoothManager(Windows.UI.Core.CoreDispatcher dispatcher)
        {
            _BluetoothDeviceList = new List<RXBluetoothDevice>();
            _ServiceProviderList = new List<RXRfcommServiceProvider>();
            LEScanner = new RXBluetoothLEScanner(this);
            RfcommScanner = new RXBluetoothRfcommScanner(this);
            Dispatcher = dispatcher;
        }


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

        public async Task<IRfcommServiceProvider> CreateRfcommServiceProviderAsync(Guid serviceId)
        {
            foreach(var serviceProvider in _ServiceProviderList)
            {
                if (serviceProvider.ServiceId == serviceId)
                {
                    return serviceProvider;
                }
            }
            var win10RfcommProvider = await RfcommServiceProvider.CreateAsync(RfcommServiceId.FromUuid(serviceId));
            RXRfcommServiceProvider provider = new RXRfcommServiceProvider(this, win10RfcommProvider);
            _ServiceProviderList.Add(provider);
            return provider;
            
        }
    }
}
