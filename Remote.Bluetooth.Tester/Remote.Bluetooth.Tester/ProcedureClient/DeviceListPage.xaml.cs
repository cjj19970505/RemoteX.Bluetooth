using Remote.Bluetooth.Tester.GattServer;
using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE;
using RemoteX.Bluetooth.LE.Gatt.Server;
using RemoteX.Bluetooth.Procedure.Client;
using RemoteX.Bluetooth.Rfcomm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.ProcedureClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceListPage : ContentPage
    {
        ObservableCollection<IBluetoothDevice> BluetoothDeviceList;

        private IManagerManager ManagerManager { get; }
        private IBluetoothManager BluetoothManager { get; }
        private IBluetoothLEScanner DeviceScanner { get; }

        RfcommFixedLengthConnectionHandler RfcommConnectionHandler { get; set; }
        public DeviceListPage()
        {
            BluetoothDeviceList = new ObservableCollection<IBluetoothDevice>();
            ManagerManager = DependencyService.Get<IManagerManager>();
            BluetoothManager = ManagerManager.BluetoothManager;
            DeviceScanner = BluetoothManager.LEScanner;
            DeviceScanner.Added += DeviceScanner_Added;
            DeviceScanner.Removed += DeviceScanner_Removed;
            DeviceScanner.Stopped += DeviceScanner_Stopped;
            DeviceScanner.EnumerationCompleted += DeviceScanner_EnumerationCompleted;

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

        private void ScanButton_Clicked(object sender, EventArgs e)
        {
            if (DeviceScanner.Status == BluetoothLEScannerState.Started)
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
            ConnectionProfile profile = new ConnectionProfile();
            var dict = new Dictionary<Guid, List<CharacteristicProfile>>();
            dict.Add(BatteryServiceWrapper.BATTERY_SERVICE_UUID, new List<CharacteristicProfile>()
            {
                new CharacteristicProfile
                {
                    Notified = true,
                    Guid = BatteryLevelCharacteristicWrapper.BATTERY_LEVEL_UUID
                }
            });
            dict.Add(DeviceInfomationServiceBuilder.SERVICE_DEVICE_INFORMATION, new List<CharacteristicProfile>()
            {
                new CharacteristicProfile
                {
                    Notified = false,
                    Guid = DeviceInfomationServiceBuilder.CHARACTERISTIC_MANUFACTURER_NAME,
                }
            });
            profile.RequiredCharacteristicGuids = dict;

            profile.RequiredServiceGuids = new List<Guid>() { Guid.Parse("4fb996ea-01dc-466c-8b95-9a018c289cef") };
            ConnectionBuilder builder = new ConnectionBuilder(BluetoothManager, profile, DeviceListView.SelectedItem as IBluetoothDevice);
            var result = await builder.StartAsync();
            result[BatteryServiceWrapper.BATTERY_SERVICE_UUID, BatteryLevelCharacteristicWrapper.BATTERY_LEVEL_UUID].OnNotified += DeviceListPage_OnNotified;
            RfcommConnectionHandler = new RfcommFixedLengthConnectionHandler(result[Guid.Parse("4fb996ea-01dc-466c-8b95-9a018c289cef")].RfcommConnection, 6);
            RfcommConnectionHandler.OnReceived += RfcommConnectionHandler_OnReceived;
            //await (DeviceListView.SelectedItem as IBluetoothDevice).GattClient.ConnectToServerAsync();
        }

        private void RfcommConnectionHandler_OnReceived(object sender, byte[] e)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < e.Length; i++)
            {
                sb.AppendFormat("{0:X2} ", e[i]);
            }
            System.Diagnostics.Debug.WriteLine("RFCOMM RECEIVE:" + sb.ToString());
        }

        private void DeviceListPage_OnNotified(object sender, byte[] e)
        {
            System.Diagnostics.Debug.WriteLine("Notified:" + e[0]);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (DeviceScanner.Status == BluetoothLEScannerState.Started)
            {
                DeviceScanner.Stop();
            }
        }

        private async void SendButton_Clicked(object sender, EventArgs e)
        {
            if(RfcommConnectionHandler == null)
            {
                return;
            }
            await RfcommConnectionHandler.SendAsync(Encoding.UTF8.GetBytes(SendEditor.Text));
        }
    }
}