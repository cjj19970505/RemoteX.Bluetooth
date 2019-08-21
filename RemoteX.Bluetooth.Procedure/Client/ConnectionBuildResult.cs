using RemoteX.Bluetooth.LE.Gatt.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.Procedure.Client
{
    public class ConnectionBuildResult
    {
        public Dictionary<IGattClientService, Dictionary<Guid, IGattClientCharacteristic>> ServiceCharacteristcLookup { get; }
        internal ConnectionBuildResult(Dictionary<IGattClientService, Dictionary<Guid, IGattClientCharacteristic>> serviceCharacteristcLookup)
        {
            ServiceCharacteristcLookup = serviceCharacteristcLookup;
        }

        public IGattClientService GetGattServiceFromGuid(Guid serviceGuid)
        {
            foreach(var service in ServiceCharacteristcLookup.Keys)
            {
                if(service.Uuid == serviceGuid)
                {
                    return service;
                }
            }
            return null;
        }
        public IGattClientCharacteristic this[Guid gattServiceGuid, Guid gattCharacteristicGuid]
        {
            get
            {
                var service = GetGattServiceFromGuid(gattServiceGuid);
                if(service == null)
                {
                    return null;
                }
                if(!ServiceCharacteristcLookup.ContainsKey(service))
                {
                    return null;
                }
                var characteristicDict = ServiceCharacteristcLookup[service];
                if(characteristicDict.ContainsKey(gattCharacteristicGuid))
                {
                    return characteristicDict[gattCharacteristicGuid];
                }
                return null;
            }
        }



    }
}
