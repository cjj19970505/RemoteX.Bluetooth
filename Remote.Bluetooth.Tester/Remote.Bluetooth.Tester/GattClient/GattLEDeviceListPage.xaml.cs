using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.GattClient
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GattLEDeviceListPage : ContentPage
	{
        ObservableCollection<IBluetoothDevice> BluetoothDeviceList;
        private IManagerManager ManagerManager { get; }
        private IBluetoothManager BluetoothManager { get; }
        private IBluetoothLEScanner DeviceScanner { get; }
        
        
        public GattLEDeviceListPage ()
		{
            BluetoothDeviceList = new ObservableCollection<IBluetoothDevice>();
            ManagerManager = DependencyService.Get<IManagerManager>();
            BluetoothManager = ManagerManager.BluetoothManager;
            DeviceScanner = BluetoothManager.LEScanner;
            
            InitializeComponent ();
            BluetoothDeviceList.Add(new TestBluetoothDevice("FUCK", 334665193540ul));
            BluetoothDeviceList.Add(new TestBluetoothDevice("FUCK", 334665193540ul));
            BluetoothDeviceList.Add(new TestBluetoothDevice("FUCK", 334665ul));
            DeviceListView.ItemsSource = BluetoothDeviceList;
        }

        private void ScanButton_Clicked(object sender, EventArgs e)
        {
            BluetoothManager.LEScanner.Start();
        }

        private void DeviceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }

        private void ConnectButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}