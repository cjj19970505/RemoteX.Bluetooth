using RemoteX.Bluetooth.LE.Gatt;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using RemoteX.Bluetooth.LE.Gatt.Server;

namespace Remote.Bluetooth.Tester.GattServer
{
    public class GattRequestViewModel: INotifyPropertyChanged
    {
        public IGattServerRequest GattServerRequest { get; }
        public string GattRequestType { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public GattRequestState GattRequestState
        {
            get
            {
                return GattServerRequest.State;
            }
        }

        public string[] ErrorCodeNames
        {
            get
            {
                return Enum.GetNames(typeof(GattErrorCode));
            }
        }

        public GattRequestViewModel(IGattServerRequest gattServerRequest)
        {
            GattServerRequest = gattServerRequest;
            GattServerRequest.StateChanged += _OnStateChanged;
            if (GattServerRequest is ICharacteristicReadRequest)
            {
                GattRequestType = "Characteristic Read Request";
            }
            else if(GattServerRequest is IDescriptorReadRequest)
            {
                GattRequestType = "Descriptor Read Request";
            }
            else if(GattServerRequest is ICharacteristicWriteRequest)
            {
                GattRequestType = "Characteristic Write Request";
            }
            else if(GattServerRequest is IDescriptorWriteRequest)
            {
                GattRequestType = "Descriptor Write Request";
            }
            else
            {
                GattRequestType = "Unknown";
            }
            
        }

        private void _OnStateChanged(object sender, GattRequestState e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GattRequestState"));
        }

        public GattRequestViewModel(string test)
        {
            GattRequestType = test;
        }

        public override string ToString()
        {
            return "YOU SON OF BITCH";
        }


    }
}
