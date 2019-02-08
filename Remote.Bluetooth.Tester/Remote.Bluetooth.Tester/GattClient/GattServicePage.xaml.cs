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
	public partial class GattServicePage : ContentPage
	{
        public ObservableCollection<IGattClientCharacteristic> Characteristics;
        IGattClientService GattService { get; }
		public GattServicePage (IGattClientService gattService)
		{
            GattService = gattService;
            Characteristics = new ObservableCollection<IGattClientCharacteristic>();
            InitializeComponent ();
            BindingContext = GattService;
            CharactersticListView.ItemsSource = Characteristics;

        }

        private async void GetCharacteristicsButton_Clicked(object sender, EventArgs e)
        {
            GetCharacteristicsButton.IsEnabled = false;
            var result = await GattService.DiscoverAllCharacteristicsAsync();
            System.Diagnostics.Debug.WriteLine(Enum.GetName(typeof(RemoteX.Bluetooth.LE.Gatt.GattErrorCode), result.ProtocolError));
            if(result.ProtocolError == RemoteX.Bluetooth.LE.Gatt.GattErrorCode.Success)
            {
                foreach(var characteristic in result.Characteristics)
                {
                    Characteristics.Add(characteristic);
                }
            }
            GetCharacteristicsButton.IsEnabled = true;
        }

        private async void EnterCharacteristicButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GattCharacteristicPage(CharactersticListView.SelectedItem as IGattClientCharacteristic));
        }

        private void CharactersticListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                EnterCharacteristicButton.IsEnabled = true;
            }
            else
            {
                EnterCharacteristicButton.IsEnabled = false;
            }
        }
    }
}