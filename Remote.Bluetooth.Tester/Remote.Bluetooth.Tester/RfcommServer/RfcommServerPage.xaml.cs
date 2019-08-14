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

namespace Remote.Bluetooth.Tester.RfcommServer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RfcommServerPage : ContentPage
    {
        IBluetoothManager BluetoothManager { get; }
        ObservableCollection<IRfcommServiceProvider> ServiceProviders;
        public RfcommServerPage()
        {
            BluetoothManager = DependencyService.Get<IManagerManager>().BluetoothManager;
            ServiceProviders = new ObservableCollection<IRfcommServiceProvider>();
            InitializeComponent();
            ProviderListView.ItemsSource = ServiceProviders;


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ServiceProviders.Clear();
            foreach (var provider in BluetoothManager.ServiceProviders)
            {
                ServiceProviders.Add(provider);
            }
        }

        private async void CreateServerProviderButton_Clicked(object sender, EventArgs e)
        {
            var provider = await BluetoothManager.CreateRfcommServiceProviderAsync(Guid.Parse("4fb996ea-01dc-466c-8b95-9a018c289cef"));
            Device.BeginInvokeOnMainThread(() =>
            {
                ServiceProviders.Add(provider);
            });
        }

        private async void ProviderListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var provider = e.SelectedItem as IRfcommServiceProvider;
            await Navigation.PushAsync(new RfcommServiceProviderPage(provider));
        }
    }
}