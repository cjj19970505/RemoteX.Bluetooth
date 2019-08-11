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

namespace Remote.Bluetooth.Tester.Rfcomm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    
    public partial class RfcommDevicePage : ContentPage
    {
        public ObservableCollection<IRfcommDeviceService> Services;
        public IBluetoothDevice BluetoothDevice { get; }
        public RfcommDevicePage(IBluetoothDevice device)
        {
            Services = new ObservableCollection<IRfcommDeviceService>();
            BluetoothDevice = device;
            InitializeComponent();
            BindingContext = device;
            ServiceListView.ItemsSource = Services;
        }

        private void ServiceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                EnterServicePageButton.IsEnabled = false;
                SelectedServiceUuidLabel.Text = "NO SELECTED SERVICE";
            }
            else
            {
                EnterServicePageButton.IsEnabled = true;
                SelectedServiceUuidLabel.Text = (ServiceListView.SelectedItem as IRfcommDeviceService).ServiceId.ToString();
            }
        }

        private async void EnterServicePageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RfcommDeviceServicePage((ServiceListView.SelectedItem as IRfcommDeviceService)));
        }

        private async void GetAllServicesButton_Clicked(object sender, EventArgs e)
        {
            Services.Clear();
            System.Diagnostics.Debug.WriteLine("START DISCOVERY SERVICE");

            var serviceResult = await BluetoothDevice.GetRfcommServicesAsync();
            System.Diagnostics.Debug.WriteLine("ERROR:" + serviceResult.Error);
            foreach(var service in serviceResult.Services)
            {
                Services.Add(service);
            }
        }
    }
}