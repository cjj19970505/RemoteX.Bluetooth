using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.GattServer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ServiceListPage : ContentPage
	{
        ObservableCollection<GattServiceModel> serviceModelList;
        BatteryServiceWrapper BatteryServiceWrapper;
        TestGattServiceWrapper TestGattServiceWrapper;

        public ServiceListPage ()
		{
            serviceModelList = new ObservableCollection<GattServiceModel>();
            InitializeComponent ();
            GattServiceListView.ItemsSource = serviceModelList;

        }

        private async void GattServiceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (GattServiceListView.SelectedItem == null)
            {
                return;
            }
            
            Page servicePage = new GattServicePage(e.SelectedItem as GattServiceModel);
            if((e.SelectedItem as GattServiceModel).GattServerService.Uuid == BluetoothUtils.ShortValueUuid(0x3432))
            {
                servicePage = new TestGattServicePage(e.SelectedItem as GattServiceModel, TestGattServiceWrapper);
            }
            else if((e.SelectedItem as GattServiceModel).GattServerService.Uuid == BatteryServiceWrapper.BATTERY_SERVICE_UUID)
            {
                servicePage = new BatteryServicePage(BatteryServiceWrapper);
            }

            GattServiceListView.SelectedItem = null;
            await Navigation.PushAsync(servicePage);
            
        }

        private void StartAdvertisingButton_Clicked(object sender, EventArgs e)
        {
            var bluetoothManager = DependencyService.Get<IManagerManager>().BluetoothManager;
            bluetoothManager.GattSever.AddService(new DeviceInfomationServiceBuilder(bluetoothManager).Build());
            BatteryServiceWrapper = new BatteryServiceWrapper(bluetoothManager);
            bluetoothManager.GattSever.AddService(BatteryServiceWrapper.GattServerService);
            TestGattServiceWrapper = new TestGattServiceWrapper(bluetoothManager, 0x3432);
            bluetoothManager.GattSever.AddService(TestGattServiceWrapper.GattServerService);
            bluetoothManager.GattSever.StartAdvertising();

            var gattServices = bluetoothManager.GattSever.Services;
            foreach(var gattService in gattServices)
            {
                serviceModelList.Add(new GattServiceModel(gattService));
            }
        }
    }
}