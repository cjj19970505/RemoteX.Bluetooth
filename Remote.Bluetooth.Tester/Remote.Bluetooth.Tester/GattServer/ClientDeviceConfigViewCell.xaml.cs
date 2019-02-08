using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.GattServer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClientDeviceConfigViewCell : ViewCell
	{
        public ClientCharacteristicConfigurationDescriptorWrapper ClientCharacteristicConfigurationDescriptorWrapper
        {
            get
            {
                return (BindingContext as ClientDeviceConfigPage.Model).ClientCharacteristicConfigurationDescriptorWrapper;
            }
        }
        public IBluetoothDevice BluetoothDevice
        {
            get
            {
                return (BindingContext as ClientDeviceConfigPage.Model).BluetoothDevice;
            }
        }

        public ClientDeviceConfigViewCell ()
		{
			InitializeComponent ();
		}

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _UpdateButtonState();
        }

        private void SetNotifyButton_Clicked(object sender, EventArgs e)
        {
            var config = ClientCharacteristicConfigurationDescriptorWrapper[BluetoothDevice];
            config.Notifications = true;
            ClientCharacteristicConfigurationDescriptorWrapper[BluetoothDevice] = config;
            _UpdateButtonState();
        }

        private void SetNotNotifyButton_Clicked(object sender, EventArgs e)
        {
            var config = ClientCharacteristicConfigurationDescriptorWrapper[BluetoothDevice];
            config.Notifications = false;
            ClientCharacteristicConfigurationDescriptorWrapper[BluetoothDevice] = config;
            _UpdateButtonState();
        }

        private void _UpdateButtonState()
        {
            if (ClientCharacteristicConfigurationDescriptorWrapper[BluetoothDevice].Notifications)
            {
                SetNotifyButton.IsEnabled = false;
                SetNotNotifyButton.IsEnabled = true;
            }
            else
            {
                SetNotifyButton.IsEnabled = true;
                SetNotNotifyButton.IsEnabled = false;
            }
        }
    }
}