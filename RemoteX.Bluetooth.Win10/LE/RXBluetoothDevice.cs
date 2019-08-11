using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteX.Bluetooth.LE.Gatt.Client;
using RemoteX.Bluetooth.Rfcomm;
using RemoteX.Bluetooth.Win10.LE.Gatt.Client;
using RemoteX.Bluetooth.Win10.Rfcomm;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;

namespace RemoteX.Bluetooth.Win10.LE
{
    internal class RXBluetoothDevice:IBluetoothDevice
    {
        public BluetoothLEDevice Win10LEDevice { get; }

        public BluetoothDevice Win10RfcommDevice { get; private set; }
        public DeviceInformation Win10DeviceInformaion { get; }
        public BluetoothManager RXBluetoothManager { get; }
        public IGattClient GattClient { get; }

        private List<RXRfcommDeviceService> _Services;
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

        public Stream RfcommInputStream { get; private set; }
        public Stream RfcommOutputStream { get; private set; }

        public RXBluetoothDevice(BluetoothManager bluetoothManager ,DeviceInformation deviceInformation)
        {
            _Services = new List<RXRfcommDeviceService>();
            RXBluetoothManager = bluetoothManager;
            Win10DeviceInformaion = deviceInformation;
            GattClient = new RXGattClient(this);
        }

        /// <summary>
        /// 这个函数以后还是想办法废掉的好
        /// 测试一下FromIdAsync到底是真的老老实实做的还是直接从本地的
        /// </summary>
        /// <returns></returns>
        public async Task RfcommConnectAsync()
        {
            Win10RfcommDevice = await BluetoothDevice.FromIdAsync(Win10DeviceInformaion.Id);
            System.Diagnostics.Debug.WriteLine("Rfcomm Connected:" + Win10RfcommDevice.Name);
            Guid RfcommChatServiceUuid = Guid.Parse("34B1CF4D-1069-4AD6-89B6-E161D79BE4D8");
            
        }

        public RXRfcommDeviceService GetRXServiceFromWin10Service(RfcommDeviceService win10Service)
        {
            RXRfcommDeviceService rxService = null;
            foreach(var service in _Services)
            {
                if(service.ServiceId == win10Service.ServiceId.Uuid)
                {
                    rxService = service as RXRfcommDeviceService;
                }
            }
            if(rxService == null)
            {
                rxService = new RXRfcommDeviceService(this, win10Service);
                _Services.Add(rxService);
            }
            return rxService;

            
        }

        public async Task<RfcommDeviceServiceResult> GetRfcommServicesAsync()
        {
            var win10ServiceResult = await Win10RfcommDevice.GetRfcommServicesAsync(BluetoothCacheMode.Uncached);
            var serviceList = (from win10Service in win10ServiceResult.Services
                               select GetRXServiceFromWin10Service(win10Service)).ToList();
            return new RfcommDeviceServiceResult()
            {
                Error = win10ServiceResult.Error.ToRXBluetoothError(),
                Services = serviceList
            };
        }

        public async Task<RfcommDeviceServiceResult> GetRfcommServicesForIdAsync(Guid serviceId)
        {

            var win10ServiceResult = await Win10RfcommDevice.GetRfcommServicesForIdAsync(RfcommServiceId.FromUuid(serviceId), BluetoothCacheMode.Uncached);
            var serviceList = (from win10Service in win10ServiceResult.Services
                               select GetRXServiceFromWin10Service(win10Service)).ToList();
            return new RfcommDeviceServiceResult()
            {
                Error = win10ServiceResult.Error.ToRXBluetoothError(),
                Services = serviceList
            };
        }

        
    }
}
