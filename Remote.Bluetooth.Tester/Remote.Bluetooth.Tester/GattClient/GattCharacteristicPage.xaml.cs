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

        private async void GetDescriptorsButton_Clicked(object sender, EventArgs e)
        {
            var result = await Characteristic.DiscoverAllCharacteristicDescriptorsAsync();
            System.Diagnostics.Debug.WriteLine("PROTOCOL_ERROR:" + result.ProtocolError);
            if (result.ProtocolError == GattErrorCode.Success)
            {
                foreach (var descriptor in result.Descriptors)
                {
                    System.Diagnostics.Debug.WriteLine(descriptor.Uuid);
                    Descriptors.Add(descriptor);
                }
            }
        }
    }

    
}