using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.LE.Gatt.Server;
using RemoteX.Bluetooth.Procedure.Server;
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
        TcpTranspondServiceWrapper TcpTranspondServiceWrapper;

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
            else if((e.SelectedItem as GattServiceModel).GattServerService.Uuid == TcpTranspondServiceWrapper.Uuid)
            {
                servicePage = new TcpTranspondServicePage(TcpTranspondServiceWrapper);
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
            //TestGattServiceWrapper = new TestGattServiceWrapper(bluetoothManager, 0x3432);
            //bluetoothManager.GattSever.AddService(TestGattServiceWrapper.GattServerService);
            TcpTranspondServiceWrapper = new TcpTranspondServiceWrapper(bluetoothManager);
            bluetoothManager.GattSever.AddService(TcpTranspondServiceWrapper.GattServerService);
            RfcommServerServiceWrapper rfcommServerServiceWrapper = new RfcommServerServiceWrapper(bluetoothManager);
            bluetoothManager.GattSever.AddService(rfcommServerServiceWrapper.GattServerService);
            bluetoothManager.GattSever.StartAdvertising();
        }

        private async void ShowDeviceListButton_Clicked(object sender, EventArgs e)
        {
            var bluetoothManager = DependencyService.Get<IManagerManager>().BluetoothManager;
            await Navigation.PushAsync(new ConnectedDevicesPage(bluetoothManager.GattSever));
        }

        private void NewServiceButton_Clicked(object sender, EventArgs e)
        {
            var bluetoothManager = DependencyService.Get<IManagerManager>().BluetoothManager;
            TestGattServiceWrapper = new TestGattServiceWrapper(bluetoothManager, 0x3432);
            bluetoothManager.GattSever.AddService(TestGattServiceWrapper.GattServerService);
        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            var bluetoothManager = DependencyService.Get<IManagerManager>().BluetoothManager;
            
            var gattServices = bluetoothManager.GattSever.Services;
            bluetoothManager.GattSever.OnServiceAdded += GattSever_OnServiceAdded;
            foreach (var gattService in gattServices)
            {
                serviceModelList.Add(new GattServiceModel(gattService));
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var bluetoothManager = DependencyService.Get<IManagerManager>().BluetoothManager;
            bluetoothManager.GattSever.OnServiceAdded -= GattSever_OnServiceAdded;
            serviceModelList.Clear();
        }

        private void GattSever_OnServiceAdded(object sender, IGattServerService e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                serviceModelList.Add(new GattServiceModel(e));
            });
        }
    }

    
}