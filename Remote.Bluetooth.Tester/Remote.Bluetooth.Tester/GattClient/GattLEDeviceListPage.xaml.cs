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
            DeviceScanner.Added += DeviceScanner_Added;
            DeviceScanner.Removed += DeviceScanner_Removed;
            DeviceScanner.Stopped += DeviceScanner_Stopped;
            DeviceScanner.EnumerationCompleted += DeviceScanner_EnumerationCompleted;

            InitializeComponent ();
            DeviceListView.ItemsSource = BluetoothDeviceList;
        }

        private void DeviceScanner_EnumerationCompleted(object sender, EventArgs e)
        {
            DeviceScanner.Stop();
        }

        private void DeviceScanner_Stopped(object sender, EventArgs e)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                ScanButton.Text = "Scan";
            });
        }

        private void DeviceScanner_Removed(object sender, IBluetoothDevice e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                BluetoothDeviceList.Remove(e);
            });
            
        }

        private void DeviceScanner_Added(object sender, IBluetoothDevice e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                BluetoothDeviceList.Add(e);
            });
            
        }

        private void ScanButton_Clicked(object sender, EventArgs e)
        {
            if(DeviceScanner.Status == BluetoothLEScannerState.Started)
            {
                DeviceScanner.Stop();
            }
            else
            {
                BluetoothDeviceList.Clear();
                BluetoothManager.LEScanner.Start();
                ScanButton.Text = "Scanning(Click To Stop)";
            }
            
        }

        private void DeviceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ConnectButton.IsEnabled = true;
        }

        private async void ConnectButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LEDevicePage(DeviceListView.SelectedItem as IBluetoothDevice));
            //await (DeviceListView.SelectedItem as IBluetoothDevice).GattClient.ConnectToServerAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if(DeviceScanner.Status == BluetoothLEScannerState.Started)
            {
                DeviceScanner.Stop();
            }
        }
    }
}