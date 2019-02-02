using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;
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
	public partial class BatteryServicePage : ContentPage
	{
        public BatteryServiceWrapper BatteryServiceWrapper;
        public BatteryServicePage (BatteryServiceWrapper batteryServiceWrapper)
		{
			InitializeComponent ();
            BatteryServiceWrapper = batteryServiceWrapper;
            BatteryValueSlider.Value = batteryServiceWrapper.BatteryLevelCharacteristicWrapper.BatteryLevel;
        }

        private void BatteryValueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            BatteryServiceWrapper.BatteryLevelCharacteristicWrapper.BatteryLevel = (int)e.NewValue;
        }
    }
}