using RemoteX.Bluetooth;
using RemoteX.Bluetooth.Rfcomm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.RfcommClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RfcommDeviceListPage : ContentPage
    {
        ObservableCollection<IBluetoothDevice> BluetoothDeviceList;
        IBluetoothManager BluetoothManager { get; }
        IManagerManager ManagerManager { get; }
        IBluetoothRfcommScanner DeviceScanner { get; }
        public RfcommDeviceListPage()
        {
            BluetoothDeviceList = new ObservableCollection<IBluetoothDevice>();

            ManagerManager = DependencyService.Get<IManagerManager>();
            BluetoothManager = ManagerManager.BluetoothManager;
            DeviceScanner = BluetoothManager.RfcommScanner;
            DeviceScanner.Added += DeviceScanner_Added;
            DeviceScanner.Removed += DeviceScanner_Removed;
            DeviceScanner.EnumerationCompleted += DeviceScanner_EnumerationCompleted;
            DeviceScanner.Stopped += DeviceScanner_Stopped;
            InitializeComponent();
            DeviceListView.ItemsSource = BluetoothDeviceList;
        }

        private void DeviceScanner_EnumerationCompleted(object sender, EventArgs e)
        {
            DeviceScanner.Stop();
        }

        private void DeviceScanner_Stopped(object sender, EventArgs e)
        {
            ScanButton.Text = "Scan";
        }

        private void DeviceScanner_Removed(object sender, IBluetoothDevice e)
        {
            BluetoothDeviceList.Remove(e);
        }

        private void DeviceScanner_Added(object sender, IBluetoothDevice e)
        {
            
            BluetoothDeviceList.Add(e);
        }

        private void DeviceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ConnectButton.IsEnabled = true;
        }

        private async void ConnectButton_Clicked(object sender, EventArgs e)
        {
            var device = (DeviceListView.SelectedItem as IBluetoothDevice);
            await device.RfcommConnectAsync();
            await Navigation.PushAsync(new RfcommDevicePage(device));

        }

        private void ScanButton_Clicked(object sender, EventArgs e)
        {
            if (DeviceScanner.Status == BluetoothRfcommScannerState.Started)
            {
                DeviceScanner.Stop();
            }
            else
            {
                BluetoothDeviceList.Clear();
                BluetoothManager.RfcommScanner.Start();
                ScanButton.Text = "Scanning(Click To Stop)";
            }
        }
    }
}