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
    public partial class RfcommConnectionPage : ContentPage
    {
        public IRfcommConnection RfcommConnection { get; }
        ObservableCollection<string> RXList { get; }
        public RfcommConnectionPage(IRfcommConnection rfcommConnection)
        {
            RXList = new ObservableCollection<string>();
            RfcommConnection = rfcommConnection;
            InitializeComponent();
            BindingContext = RfcommConnection;
        }

        private async void SendButton_Clicked(object sender, EventArgs e)
        {
            var sendText = SendEntry.Text;
            var sendBuffer = Encoding.UTF8.GetBytes(sendText);
            await RfcommConnection.OutputStream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            await RfcommConnection.OutputStream.FlushAsync();
        }
    }
}