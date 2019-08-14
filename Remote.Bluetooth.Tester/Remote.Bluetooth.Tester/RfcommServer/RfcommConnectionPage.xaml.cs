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
            ReceivedListView.ItemsSource = RXList;
            Task t = ReadInputSreamAsync();
        }

        private async void SendButton_Clicked(object sender, EventArgs e)
        {
            var sendText = SendEntry.Text;
            var sendBuffer = Encoding.UTF8.GetBytes(sendText);
            await RfcommConnection.OutputStream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            await RfcommConnection.OutputStream.FlushAsync();
        }

        Task ReadInputSreamAsync()
        {
            return Task.Run(() =>
            {
                bool disconnected = false;
                while (true)
                {
                    int readBufferSize = 255;
                    byte[] buffer = new byte[readBufferSize];
                    var readSize = RfcommConnection.InputStream.Read(buffer, 0, readBufferSize);
                    if(readSize == 0)
                    {
                        disconnected = true;
                        break;
                    }
                    
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < readSize; i++)
                    {
                        sb.AppendFormat("{0:X2} ", buffer[i]);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        RXList.Add(sb.ToString());
                    });
                }
                RfcommConnection.Dispose();
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                });
            });
        }
    }
}