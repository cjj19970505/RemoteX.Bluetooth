using RemoteX.Bluetooth;
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
            await BluetoothDevice.GattClient.ConnectToServerAsync();
            var serviceResult = await BluetoothDevice.GattClient.DiscoverAllPrimaryServiceAsync();
            if(serviceResult.ProtocolError == RemoteX.Bluetooth.LE.Gatt.GattErrorCode.Success)
            {
                System.Diagnostics.Debug.WriteLine("SUCCESS");
                foreach(var service in serviceResult.Services)
                {
                    Services.Add(service);
                }
            }
        }
    }
}