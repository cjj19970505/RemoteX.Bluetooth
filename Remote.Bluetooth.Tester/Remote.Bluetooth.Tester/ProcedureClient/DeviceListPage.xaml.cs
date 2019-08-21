﻿using Remote.Bluetooth.Tester.GattServer;
using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE;
using RemoteX.Bluetooth.LE.Gatt.Server;
using RemoteX.Bluetooth.Procedure.Client;
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
            ConnectionBuilder builder = new ConnectionBuilder(BluetoothManager, profile, DeviceListView.SelectedItem as IBluetoothDevice);
            var result = await builder.StartAsync();
            result[BatteryServiceWrapper.BATTERY_SERVICE_UUID, BatteryLevelCharacteristicWrapper.BATTERY_LEVEL_UUID].OnNotified += DeviceListPage_OnNotified;

            //await (DeviceListView.SelectedItem as IBluetoothDevice).GattClient.ConnectToServerAsync();
        }

        private void DeviceListPage_OnNotified(object sender, byte[] e)
        {
            System.Diagnostics.Debug.WriteLine("Notified:"+e[0]);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (DeviceScanner.Status == BluetoothLEScannerState.Started)
            {
                DeviceScanner.Stop();
            }
        }
    }
}