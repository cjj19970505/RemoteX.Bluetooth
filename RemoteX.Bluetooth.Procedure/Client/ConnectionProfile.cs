using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.Procedure.Client
{
    public class ConnectionProfile
    {
        public Dictionary<Guid, List<CharacteristicProfile>> RequiredCharacteristicGuids;
        public List<Guid> RequiredServiceGuids;

        public ConnectionProfile()
        {
            RequiredCharacteristicGuids = new Dictionary<Guid, List<CharacteristicProfile>>();
            RequiredServiceGuids = new List<Guid>();
        }
    }

    public struct CharacteristicProfile
    {
        public bool Notified;
        public Guid Guid;
    }

    
}
