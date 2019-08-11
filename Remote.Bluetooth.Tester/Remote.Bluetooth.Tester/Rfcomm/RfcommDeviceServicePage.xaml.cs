using RemoteX.Bluetooth.Rfcomm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.Rfcomm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RfcommDeviceServicePage : ContentPage
    {
        ObservableCollection<string> RXList;
        IRfcommDeviceService RfcommDeviceService { get; }
        public RfcommDeviceServicePage(IRfcommDeviceService service)
        {
            RfcommDeviceService = service;
            RXList = new ObservableCollection<string>();
            InitializeComponent();
            RXListView.ItemsSource = RXList;
            BindingContext = RfcommDeviceService;
        }

        private async void ConnectButton_Clicked(object sender, EventArgs e)
        {
            while(true)
            {
                try
                {
                    await RfcommDeviceService.ConnectAsync();
                    ConnectButton.Text = "Connected";
                    var readTask = ReadInputSreamAsync();
                    break;
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine("RECONNECT "+ exception.HResult);
                }
            }
        }

        Task ReadInputSreamAsync()
        {
            return Task.Run(() =>
            {
                while(true)
                {
                    int readBufferSize = 255;
                    byte[] buffer = new byte[readBufferSize];
                    var readSize = RfcommDeviceService.InputStream.Read(buffer, 0, readBufferSize);
                    StringBuilder sb = new StringBuilder();
                    for(int i = 0;i<readSize;i++)
                    {
                        sb.AppendFormat("{0:X2} ", buffer[i]);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        RXList.Add(sb.ToString());
                    });
                }
                
            });
        }
    }
}