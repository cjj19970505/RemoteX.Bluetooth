﻿using RemoteX.Bluetooth.LE;
using RemoteX.Bluetooth.LE.Gatt;
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

        public ServiceListPage ()
		{
            serviceModelList = new ObservableCollection<GattServiceModel>
            {
            };
            InitializeComponent ();
            GattServiceListView.ItemsSource = serviceModelList;

        }

        private async void GattServiceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (GattServiceListView.SelectedItem == null)
            {
                return;
            }

            var servicePage = new GattServicePage(e.SelectedItem as GattServiceModel);

            GattServiceListView.SelectedItem = null;
            await Navigation.PushAsync(servicePage);
            
        }

        private void StartAdvertisingButton_Clicked(object sender, EventArgs e)
        {
            var bluetoothManager = DependencyService.Get<IManagerManager>().BluetoothManager;
            bluetoothManager.GattSever.AddService(new DeviceInfomationServiceBuilder(bluetoothManager).Build());
            BatteryServiceWrapper = new BatteryServiceWrapper(bluetoothManager);
            bluetoothManager.GattSever.AddService(BatteryServiceWrapper.GattServerService);
            bluetoothManager.GattSever.StartAdvertising();

            var gattServices = bluetoothManager.GattSever.Services;
            foreach(var gattService in gattServices)
            {
                serviceModelList.Add(new GattServiceModel(gattService));
            }
        }
    }
}