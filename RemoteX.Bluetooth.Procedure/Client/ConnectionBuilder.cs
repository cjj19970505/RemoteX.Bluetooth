using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RemoteX.Bluetooth.LE.Gatt.Client;
using System.Linq;

namespace RemoteX.Bluetooth.Procedure.Client
{
    public class ConnectionBuilder
    {

        public ConnectionProfile ConnectionProfile { get; set; }
        public IBluetoothManager BluetoothManager { get; }
        public IBluetoothDevice RemoteDevice { get; }
        public ConnectionBuilder(IBluetoothManager bluetoothManager, ConnectionProfile profile, IBluetoothDevice remoteDevice)
        {
            ConnectionProfile = profile;
            RemoteDevice = remoteDevice;
        }

        public async Task StartAsync()
        {
            await RemoteDevice.GattClient.ConnectToServerAsync();
            var serviceResult = await RemoteDevice.GattClient.DiscoverAllPrimaryServiceAsync();
            if (serviceResult.ProtocolError != LE.Gatt.GattErrorCode.Success)
            {
                throw new Exception("无法获得LE服务器的Service");
            }
            Dictionary<Guid, IGattClientService> allFoundGattServiceDict = new Dictionary<Guid, IGattClientService>();
            foreach (var foundService in serviceResult.Services)
            {
                allFoundGattServiceDict.Add(foundService.Uuid, foundService);
            }
            List<IGattClientService> gattServiceList = new List<IGattClientService>();
            foreach (var profileGattServiceGuid in ConnectionProfile.RequiredCharacteristicGuids.Keys)
            {
                if (!allFoundGattServiceDict.ContainsKey(profileGattServiceGuid))
                {
                    throw new Exception("Server中可能没有要求的Service");
                }
                else
                {
                    gattServiceList.Add(allFoundGattServiceDict[profileGattServiceGuid]);
                }
            }

            Dictionary<IGattClientService, List<IGattClientCharacteristic>> serviceCharacteristicDict = new Dictionary<IGattClientService, List<IGattClientCharacteristic>>();
            foreach (var gattService in gattServiceList)
            {
                var characteristics = await gattService.DiscoverAllCharacteristicsAsync();
                if(characteristics.ProtocolError != LE.Gatt.GattErrorCode.Success)
                {
                    throw new Exception("获取Characteristic失败");
                }
                Dictionary<Guid, IGattClientCharacteristic> allFoundGattCharacteristicDict = new Dictionary<Guid, IGattClientCharacteristic>();
                foreach(var chara in characteristics.Characteristics)
                {
                    allFoundGattCharacteristicDict.Add(chara.Uuid, chara);
                }
                List<IGattClientCharacteristic> characteristicsList = new List<IGattClientCharacteristic>();
                var inProfileCharacteristicGuidList = from characteristicProfile in ConnectionProfile.RequiredCharacteristicGuids[gattService.Uuid]
                                                      select characteristicProfile.Guid;
                
                foreach (var profileCharacteristicGuid in inProfileCharacteristicGuidList)
                {
                    if (!allFoundGattCharacteristicDict.ContainsKey(profileCharacteristicGuid))
                    {
                        throw new Exception("Service中没有要求的Characteristic");
                    }
                    else
                    {
                        characteristicsList.Add(allFoundGattCharacteristicDict[profileCharacteristicGuid]);
                    }
                }
                serviceCharacteristicDict.Add(gattService, characteristicsList);

            }



        }
    }
}
