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
	public partial class ClientDeviceConfigPage : ContentPage
	{
        ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper { get; }
        
        IGattServer GattServer { get; }

        ObservableCollection<Model> ConnectedDeviceList;
        public ClientDeviceConfigPage (IGattServer gattServer, ClientCharacteristicConfigurationDescriptorWrapper clientCharacteristicConfigurationDescriptorWrapper)
		{
            ConnectedDeviceList = new ObservableCollection<Model>();
            GattServer = gattServer;
            ClientCharacteristicConfigurationDescriptorWrapper = clientCharacteristicConfigurationDescriptorWrapper;
            InitializeComponent ();
            DeviceListView.ItemsSource = ConnectedDeviceList;
            DeviceListView.ItemAppearing += DeviceListView_ItemAppearing;
        }

        private void DeviceListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var connectedDevices = GattServer.ConnectedDevices;
            foreach (var connectedDevice in connectedDevices)
            {
                ConnectedDeviceList.Add(new Model(connectedDevice, ClientCharacteristicConfigurationDescriptorWrapper));
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
            Model readyToDeleteModel = null;
            foreach(var model in ConnectedDeviceList)
            {
                if(model.BluetoothDevice == e)
                {
                    readyToDeleteModel = model;
                }
            }
            if (readyToDeleteModel != null)
            {
                ConnectedDeviceList.Remove(readyToDeleteModel);
            }
        }

        private void GattServer_DeviceConnected(object sender, IBluetoothDevice e)
        {
            ConnectedDeviceList.Add(new Model(e, ClientCharacteristicConfigurationDescriptorWrapper));
        }

        public class Model
        {
            public IBluetoothDevice BluetoothDevice { get; }
            public ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper { get; }
            public string Name
            {
                get
                {
                    return BluetoothDevice.Name;
                }
            }
            public ulong Address
            {
                get
                {
                    return BluetoothDevice.Address;
                }
            }
            public Model(IBluetoothDevice bluetoothDevice, ClientCharacteristicConfigurationDescriptorWrapper clientCharacteristicConfigurationDescriptorWrapper)
            {
                BluetoothDevice = bluetoothDevice;
                ClientCharacteristicConfigurationDescriptorWrapper = clientCharacteristicConfigurationDescriptorWrapper;
            }
        }
    }
}