using RemoteX.Bluetooth;
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
	public partial class ConnectedDevicesPage : ContentPage
	{
        public IGattServer GattServer { get; }
        ObservableCollection<IBluetoothDevice> ConnectedDeviceList;
        public ConnectedDevicesPage (IGattServer gattServer)
		{
            GattServer = gattServer;
            ConnectedDeviceList = new ObservableCollection<IBluetoothDevice>();

            InitializeComponent ();
            DeviceListView.ItemsSource = ConnectedDeviceList;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var connectedDevices = GattServer.ConnectedDevices;
            foreach(var connectedDevice in connectedDevices)
            {
                ConnectedDeviceList.Add(connectedDevice);
            }
            GattServer.DeviceConnected += GattServer_DeviceConnected;
            GattServer.DeviceDisconnected += GattServer_DeviceDisconnected;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GattServer.DeviceConnected -= GattServer_DeviceConnected;
            GattServer.DeviceDisconnected -= GattServer_DeviceDisconnected;
        }

        private void GattServer_DeviceDisconnected(object sender, IBluetoothDevice e)
        {
            ConnectedDeviceList.Remove(e);
        }

        private void GattServer_DeviceConnected(object sender, IBluetoothDevice e)
        {
            ConnectedDeviceList.Add(e);
        }
    }
}