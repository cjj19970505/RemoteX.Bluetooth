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
    public partial class RfcommServiceProviderPage : ContentPage
    {
        public ObservableCollection<IRfcommConnection> Connections;
        public IRfcommServiceProvider ServiceProvider { get; }
        public RfcommServiceProviderPage(IRfcommServiceProvider provider)
        {
            Connections = new ObservableCollection<IRfcommConnection>();
            ServiceProvider = provider;
            ServiceProvider.OnConnectionReceived += ServiceProvider_OnConnectionReceived;
            InitializeComponent();
            BindingContext = ServiceProvider;
            ConnectedDeviceListView.ItemsSource = Connections;

        }

        private void ServiceProvider_OnConnectionReceived(object sender, IRfcommConnection e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Connections.Add(e);
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Connections.Clear();
            foreach(var connection in ServiceProvider.Connections)
            {
                Connections.Add(connection);
            }
        }

        private void StartAdvertiseButton_Clicked(object sender, EventArgs e)
        {
            ServiceProvider.StartAdvertising();
        }

        private async void ConnectedDeviceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new RfcommConnectionPage(e.SelectedItem as IRfcommConnection));
        }
    }
}