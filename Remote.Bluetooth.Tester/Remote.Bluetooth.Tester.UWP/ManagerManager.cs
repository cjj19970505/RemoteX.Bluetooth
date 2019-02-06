using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteX.Bluetooth;
using RemoteX.Bluetooth.Win10;

[assembly: Xamarin.Forms.Dependency(typeof(Remote.Bluetooth.Tester.UWP.ManagerManager))]
namespace Remote.Bluetooth.Tester.UWP
{
    public class ManagerManager : IManagerManager
    {
        private IBluetoothManager _BluetoothManager;
        public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }
        public IBluetoothManager BluetoothManager
        {
            get
            {
                if(_BluetoothManager == null)
                {
                    if(Dispatcher == null)
                    {
                        throw new Exception("Dispatcher Not Set");
                    }
                    _BluetoothManager = new BluetoothManager(Dispatcher);
                }
                return _BluetoothManager;
            }
        }
    }
}
