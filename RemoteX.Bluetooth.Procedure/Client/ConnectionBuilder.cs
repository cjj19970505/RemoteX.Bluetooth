using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RemoteX.Bluetooth.LE.Gatt.Client;
using System.Linq;
using System.Threading;
using RemoteX.Bluetooth.Rfcomm;

namespace RemoteX.Bluetooth.Procedure.Client
{
    public class ConnectionBuilder
    {
        public ConnectionProfile ConnectionProfile { get; }
        public ConnectionProfile RealUsingConnectionProfile { get; set; }
        public IBluetoothManager BluetoothManager { get; }
        public IBluetoothDevice RemoteDevice { get; }

        private Thread _WaitingForRfcommScanResultThread;
        private string _WantedDeviceName;
        private IBluetoothDevice _TargetFoundDevice;
        public ConnectionBuilder(IBluetoothManager bluetoothManager, ConnectionProfile profile, IBluetoothDevice remoteDevice)
        {
            BluetoothManager = bluetoothManager;
            ConnectionProfile = profile;
            RealUsingConnectionProfile = new ConnectionProfile
            {
                RequiredCharacteristicGuids = new Dictionary<Guid, List<CharacteristicProfile>>(ConnectionProfile.RequiredCharacteristicGuids),
                RequiredServiceGuids = new List<Guid>(ConnectionProfile.RequiredServiceGuids)
            };
            RealUsingConnectionProfile.RequiredCharacteristicGuids.Add(Constants.RfcommServerServiceGuid, new List<CharacteristicProfile>()
            {
                new CharacteristicProfile()
                {
                    Guid = Constants.RfcommServerAddressCharacteristicGuid
                },
                new CharacteristicProfile()
                {
                    Guid = Constants.RfcommServerNameCharacteristicGuid
                }
            });
            RemoteDevice = remoteDevice;
        }

        public async Task<ConnectionBuildResult> StartAsync()
        {
            if(BluetoothManager.LEScanner.Status == LE.BluetoothLEScannerState.Started)
            {
                BluetoothManager.LEScanner.Stop();
            }
            
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
            foreach (var profileGattServiceGuid in RealUsingConnectionProfile.RequiredCharacteristicGuids.Keys)
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

            Dictionary<IGattClientService, Dictionary<Guid, IGattClientCharacteristic>> serviceCharacteristicDict = new Dictionary<IGattClientService, Dictionary<Guid, IGattClientCharacteristic>>();
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
                Dictionary<Guid, IGattClientCharacteristic> characteristicsDict = new Dictionary<Guid, IGattClientCharacteristic>();
                var inProfileCharacteristicGuidList = from characteristicProfile in RealUsingConnectionProfile.RequiredCharacteristicGuids[gattService.Uuid]
                                                      select characteristicProfile.Guid;
                
                foreach (var profileCharacteristicGuid in inProfileCharacteristicGuidList)
                {
                    if (!allFoundGattCharacteristicDict.ContainsKey(profileCharacteristicGuid))
                    {
                        throw new Exception("Service中没有要求的Characteristic");
                    }
                    else
                    {
                        characteristicsDict.Add(profileCharacteristicGuid, allFoundGattCharacteristicDict[profileCharacteristicGuid]);
                        
                    }
                }
                serviceCharacteristicDict.Add(gattService, characteristicsDict);
                foreach(var charateristicProfile in RealUsingConnectionProfile.RequiredCharacteristicGuids[gattService.Uuid])
                {
                    if(charateristicProfile.Notified)
                    {
                        var result = await characteristicsDict[charateristicProfile.Guid].GattCharacteristicConfiguration.SetValueAsync(true, false);
                        if(result.ProtocolError != LE.Gatt.GattErrorCode.Success)
                        {
                            throw new Exception("无法打开Notify");
                        }
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("LE连接过程完成");
            var noRfcommResult = new ConnectionBuildResult(serviceCharacteristicDict);

            var readRfcommAddressResult = await noRfcommResult[Constants.RfcommServerServiceGuid, Constants.RfcommServerAddressCharacteristicGuid].ReadCharacteristicValueAsync();
            if(readRfcommAddressResult.ProtocolError != LE.Gatt.GattErrorCode.Success)
            {
                throw new Exception("无法读取Rfcomm的MacAddress");
            }
            var rfcommMacAddress = BitConverter.ToUInt64(readRfcommAddressResult.Value, 0);

            var readRfcommNameResult = await noRfcommResult[Constants.RfcommServerServiceGuid, Constants.RfcommServerNameCharacteristicGuid].ReadCharacteristicValueAsync();
            if (readRfcommAddressResult.ProtocolError != LE.Gatt.GattErrorCode.Success)
            {
                throw new Exception("无法读取Rfcomm的Name");
            }
            var rfcommName = Encoding.UTF8.GetString(readRfcommNameResult.Value);
            _WantedDeviceName = rfcommName;
            System.Diagnostics.Debug.WriteLine("对方的Rfcomm地址是：" + BluetoothUtils.AddressInt64ToString(rfcommMacAddress) + " Name:"+ rfcommName);
            var rfcommScanner = BluetoothManager.RfcommScanner;
            rfcommScanner.Added += RfcommScanner_Added;
            rfcommScanner.Stopped += RfcommScanner_Stopped;
            _WaitingForRfcommScanResultThread = Thread.CurrentThread;
            rfcommScanner.Start();
            try
            {
                Thread.Sleep(Timeout.Infinite);
            }
            catch(ThreadInterruptedException)
            {
                rfcommScanner.Added -= RfcommScanner_Added;
                rfcommScanner.Stopped -= RfcommScanner_Stopped;
                if (_TargetFoundDevice == null)
                {
                    throw new Exception("没有找到Rfcomm设备");
                }
                
            }
            Dictionary<Guid, IRfcommDeviceService> inProfileAndFoundRfcommServices = new Dictionary<Guid, IRfcommDeviceService>();
            foreach(var inProfileRfcommServiceId in RealUsingConnectionProfile.RequiredServiceGuids)
            {
                await _TargetFoundDevice.RfcommConnectAsync();
                var rfcommServiceResult = await _TargetFoundDevice.GetRfcommServicesForIdAsync(inProfileRfcommServiceId);
                if(rfcommServiceResult.Error != BluetoothError.Success || rfcommServiceResult.Services.Count == 0)
                {
                    throw new Exception("没有找到Rfcomm Service");
                }
                else
                {
                    await rfcommServiceResult.Services[0].ConnectAsync();
                    inProfileAndFoundRfcommServices.Add(inProfileRfcommServiceId, rfcommServiceResult.Services[0]);
                }
            }
            var finalResult = noRfcommResult;
            finalResult.RfcommServiceLookup = inProfileAndFoundRfcommServices;

            return finalResult;
        }

        private void RfcommScanner_Stopped(object sender, EventArgs e)
        {
            _WaitingForRfcommScanResultThread.Interrupt();
        }

        private void RfcommScanner_Added(object sender, IBluetoothDevice e)
        {
            if(e.Name == _WantedDeviceName)
            {
                _TargetFoundDevice = e;
                (sender as IBluetoothRfcommScanner).Stop();
            }
        }
    }
}
