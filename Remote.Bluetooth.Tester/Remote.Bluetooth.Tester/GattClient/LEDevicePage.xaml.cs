﻿using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt.Client;
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
	public partial class LEDevicePage : ContentPage
	{
        public IBluetoothDevice BluetoothDevice { get; }
        public ObservableCollection<IGattClientService> Services;
        public LEDevicePage (IBluetoothDevice bluetoothDevice)
		{
            BluetoothDevice = bluetoothDevice;
            Services = new ObservableCollection<IGattClientService>();
            InitializeComponent ();
            ServiceListView.ItemsSource = Services;
            BindingContext = BluetoothDevice;
        }

        private async void GetAllServicesButton_Clicked(object sender, EventArgs e)
        {
            GetAllServicesButton.IsEnabled = false;
            await BluetoothDevice.GattClient.ConnectToServerAsync();
            GattServiceResult serviceResult = new GattServiceResult();
            bool failed = true;
            while (failed)
            {
                try
                {
                    serviceResult = await BluetoothDevice.GattClient.DiscoverAllPrimaryServiceAsync();
                    failed = false;
                    System.Diagnostics.Debug.WriteLine("FAILED");
                }
                catch (Exception)
                {

                }
            }
            
            if(serviceResult.ProtocolError == RemoteX.Bluetooth.LE.Gatt.GattErrorCode.Success)
            {
                System.Diagnostics.Debug.WriteLine("SUCCESS");
                foreach(var service in serviceResult.Services)
                {
                    Services.Add(service);
                }
            }
            GetAllServicesButton.IsEnabled = true;
        }

        private async void EnterServicePageButton_Clicked(object sender, EventArgs e)
        {
            if(ServiceListView.SelectedItem == null)
            {
                return;
            }
            var service = ServiceListView.SelectedItem as IGattClientService;
            await Navigation.PushAsync(new GattServicePage(service));
        }

        private void ServiceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem == null)
            {
                EnterServicePageButton.IsEnabled = false;
                SelectedServiceUuidLabel.Text = "NO SELECTED SERVICE";
            }
            else
            {
                EnterServicePageButton.IsEnabled = true;
                SelectedServiceUuidLabel.Text = (ServiceListView.SelectedItem as IGattClientService).Uuid.ToString();
            }
        }
    }
}