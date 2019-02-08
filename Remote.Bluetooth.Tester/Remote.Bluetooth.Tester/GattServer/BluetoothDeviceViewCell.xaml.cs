using RemoteX.Bluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.GattServer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BluetoothDeviceViewCell : ViewCell
	{
        public IBluetoothDevice BluetoothDevice
        {
            get
            {
                return BindingContext as IBluetoothDevice;
            }
        }
		public BluetoothDeviceViewCell ()
		{
			InitializeComponent ();
		}
        
        
	}
}