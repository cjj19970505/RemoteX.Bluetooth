using RemoteX.Bluetooth.LE.Att;
using RemoteX.Bluetooth.LE.Gatt.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace RemoteX.Bluetooth.Win10.LE.Gatt.Client
{
    internal class RXGattClient:IGattClient
    {
        
        public RXBluetoothDevice BluetoothDevice { get; }
        public BluetoothLEDevice Win10LEDevice { get; private set; }
        private List<IGattClientService> _GattServiceList;

        /// <summary>
        /// 毕竟每个Service声明，Charistic和Descriptor都是唯一的，但都是Attribute，所以通过储存Attribute的方式来管理这些东西
        /// </summary>
        private List<IAttribute> _AttributeList;

        public RXGattClient(RXBluetoothDevice bluetoothDevice)
        {
            _GattServiceList = new List<IGattClientService>();
            _AttributeList = new List<IAttribute>();
            BluetoothDevice = bluetoothDevice;
        }

        public async Task ConnectToServerAsync()
        {
            Win10LEDevice = await BluetoothLEDevice.FromIdAsync(BluetoothDevice.Win10DeviceInformaion.Id);
        }

        public async Task<GattServiceResult> DiscoverAllPrimaryServiceAsync()
        {
            var win10ServiceResult = await Win10LEDevice.GetGattServicesAsync();

            GattServiceResult rxServiceResult = new GattServiceResult();
            
            if (win10ServiceResult.Status == GattCommunicationStatus.Success)
            {
                
                rxServiceResult.ProtocolError = Bluetooth.LE.Gatt.GattErrorCode.Success;
                List<RXGattClientService> rxGattClientServices = new List<RXGattClientService>();
                System.Diagnostics.Debug.WriteLine("FOUND " + win10ServiceResult.Services.Count + " Services");
                foreach(var win10Service in win10ServiceResult.Services)
                {
                    rxGattClientServices.Add(GetRXServiceFromWin10GattService(win10Service));

                }
                rxServiceResult.Services = rxGattClientServices.ToArray();
            }
            else if(win10ServiceResult.Status == GattCommunicationStatus.ProtocolError)
            {
                rxServiceResult.ProtocolError = (Bluetooth.LE.Gatt.GattErrorCode)(win10ServiceResult.ProtocolError);
            }
            else if(win10ServiceResult.Status == GattCommunicationStatus.Unreachable)
            {
                throw new NotImplementedException("UNREACHABLE");
                
            }
            else if(win10ServiceResult.Status == GattCommunicationStatus.AccessDenied)
            {
                throw new NotImplementedException("ACCESS_DENIED");
            }
            return rxServiceResult;
        }

        public RXGattClientService GetRXServiceFromWin10GattService(GattDeviceService win10Service)
        {
            RXGattClientService rxService = null;
            foreach(var attribute in _AttributeList)
            {
                if(attribute.AttributeHandle == win10Service.AttributeHandle)
                {
                    rxService = attribute as RXGattClientService;
                }
            }
            if(rxService == null)
            {
                rxService = new RXGattClientService(this, win10Service);
                _AttributeList.Add(rxService);
            }
            return rxService;
        }

        public RXGattClientCharacteristic GetRXCharacteristicFromWin10Characteristic(GattCharacteristic win10Characteristic)
        {
            RXGattClientCharacteristic rxCharacteristic = null;
            foreach (var attribute in _AttributeList)
            {
                if (attribute.AttributeHandle == win10Characteristic.AttributeHandle)
                {
                    rxCharacteristic = attribute as RXGattClientCharacteristic;
                }
            }
            if(rxCharacteristic == null)
            {
                rxCharacteristic = new RXGattClientCharacteristic(this, win10Characteristic);
                _AttributeList.Add(rxCharacteristic);
            }
            return rxCharacteristic;
        }

        public RXGattClientDescriptor GetRXDescriptorFromWin10Descriptor(GattDescriptor win10Descriptor)
        {
            RXGattClientDescriptor rxDescriptor = null;
            foreach(var attribute in _AttributeList)
            {
                if(attribute.AttributeHandle == win10Descriptor.AttributeHandle)
                {
                    rxDescriptor = attribute as RXGattClientDescriptor;
                }
            }
            if(rxDescriptor == null)
            {
                rxDescriptor = new RXGattClientDescriptor(win10Descriptor);
                _AttributeList.Add(rxDescriptor);
            }
            return rxDescriptor;
        }
    }
}
