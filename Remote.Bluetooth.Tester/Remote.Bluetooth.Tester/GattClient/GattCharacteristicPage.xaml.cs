using RemoteX.Bluetooth.LE.Gatt;
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
	public partial class GattCharacteristicPage : ContentPage
	{
        IGattClientCharacteristic Characteristic { get; }
        ObservableCollection<IGattClientDescriptor> Descriptors;

        public GattCharacteristicPage (IGattClientCharacteristic characteristic)
		{
            Descriptors = new ObservableCollection<IGattClientDescriptor>();
            Characteristic = characteristic;
            InitializeComponent ();
            BindingContext = Characteristic;
            DescriptorListView.ItemsSource = Descriptors;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Characteristic.OnNotified += Characteristic_OnNotified;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Characteristic.OnNotified -= Characteristic_OnNotified;
        }

        private void Characteristic_OnNotified(object sender, byte[] e)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var data in e)
            {
                sb.AppendFormat("{0:X2}", data);
                sb.Append(" ");
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                WriteEditor.Text = sb.ToString();
            });
        }

        private async void GetDescriptorsButton_Clicked(object sender, EventArgs e)
        {
            Descriptors.Clear();
            var result = await Characteristic.DiscoverAllCharacteristicDescriptorsAsync();
            System.Diagnostics.Debug.WriteLine("PROTOCOL_ERROR:" + result.ProtocolError);
            if (result.ProtocolError == GattErrorCode.Success)
            {
                foreach (var descriptor in result.Descriptors)
                {
                    Descriptors.Add(descriptor);
                }
            }
        }

        private async void WriteButton_Clicked(object sender, EventArgs e)
        {
            var valueBytes = Encoding.UTF8.GetBytes(WriteEditor.Text);
            var result = await Characteristic.WriteAsync(valueBytes);
            System.Diagnostics.Debug.WriteLine("WriteCharacteristicResult:"+result);
        }

        private async void WriteWithoutResponseButton_Clicked(object sender, EventArgs e)
        {
            var valueBytes = Encoding.UTF8.GetBytes(WriteEditor.Text);
            var result = await Characteristic.WriteWithoutResponseAsync(valueBytes);
            System.Diagnostics.Debug.WriteLine("WriteCharacteristicResult:" + result);
        }

        private void SetNotifyButton_Clicked(object sender, EventArgs e)
        {
            Characteristic.GattCharacteristicConfiguration.SetValueAsync(true, false);
        }

        private void SetNotNotifyButton_Clicked(object sender, EventArgs e)
        {
            Characteristic.GattCharacteristicConfiguration.SetValueAsync(false, false);
        }
    }

    
}