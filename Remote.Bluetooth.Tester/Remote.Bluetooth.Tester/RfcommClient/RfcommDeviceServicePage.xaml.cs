using RemoteX.Bluetooth.Rfcomm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.RfcommClient
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
                    var readSize = RfcommDeviceService.RfcommConnection.InputStream.Read(buffer, 0, readBufferSize);
                    if(readSize == 0)
                    {
                        break;
                    }
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
                RfcommDeviceService.RfcommConnection.Dispose();
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                });
            });
        }

        private async void SendButton_Clicked(object sender, EventArgs e)
        {
            var sendText = SendEntry.Text;
            if(sendText == null)
            {
                sendText = "SUD";
            }
            var strBuffer = Encoding.UTF8.GetBytes(sendText);
            var lenBuffer = BitConverter.GetBytes((UInt32)sendText.Length);
            lenBuffer = new byte[] { lenBuffer[3], lenBuffer[2], lenBuffer[1], lenBuffer[0] };
            List<byte> bufferList = new List<byte>();
            //bufferList.AddRange(lenBuffer);
            bufferList.AddRange(strBuffer);
            //bufferList = new List<byte>(new byte[] { 0, 0, 0, 3, 70, 70, 70 });
            
            await RfcommDeviceService.RfcommConnection.OutputStream.WriteAsync(bufferList.ToArray(), 0, bufferList.Count);
            await RfcommDeviceService.RfcommConnection.OutputStream.FlushAsync();
            System.Diagnostics.Debug.WriteLine("SENT");
            //await RfcommDeviceService.TrySend();
        }
    }
}