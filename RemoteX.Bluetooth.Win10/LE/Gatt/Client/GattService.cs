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
    internal class RXGattClientService : IGattClientService
    {
        public GattDeviceService Win10GattService { get; }
        public RXGattClient RXGattClient { get; }

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

        public RXGattClientService(RXGattClient rxGattClient, GattDeviceService win10GattService)
        {
            RXGattClient = rxGattClient;
            Win10GattService = win10GattService;
        }

        public async Task<Bluetooth.LE.Gatt.Client.GattCharacteristicsResult> DiscoverAllCharacteristicsAsync()
        {
            
            var win10Result = await Win10GattService.GetCharacteristicsAsync();
            Bluetooth.LE.Gatt.Client.GattCharacteristicsResult rxResult = new Bluetooth.LE.Gatt.Client.GattCharacteristicsResult();
            if(win10Result.Status == GattCommunicationStatus.Success)
            {
                rxResult.ProtocolError = GattErrorCode.Success;
                List<RXGattClientCharacteristic> rxCharacteristicList = new List<RXGattClientCharacteristic>();
                foreach(var win10Characteristic in win10Result.Characteristics)
                {
                    rxCharacteristicList.Add(RXGattClient.GetRXCharacteristicFromWin10Characteristic(win10Characteristic));
                }
                rxResult.Characteristics = rxCharacteristicList.ToArray();
            }
            else if(win10Result.Status == GattCommunicationStatus.ProtocolError)
            {
                rxResult.ProtocolError = (Bluetooth.LE.Gatt.GattErrorCode)(win10Result.ProtocolError);
            }
            else if (win10Result.Status == GattCommunicationStatus.Unreachable)
            {
                throw new NotImplementedException("UNREACHABLE");

            }
            else if (win10Result.Status == GattCommunicationStatus.AccessDenied)
            {
                throw new NotImplementedException("ACCESS_DENIED");
            }
            return rxResult;
        }
    }
}
