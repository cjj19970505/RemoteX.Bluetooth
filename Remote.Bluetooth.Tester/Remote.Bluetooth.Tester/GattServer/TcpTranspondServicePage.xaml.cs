using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.GattServer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TcpTranspondServicePage : ContentPage
	{
        public ObservableCollection<string> ReceivedMessages;
        public TcpTranspondServiceWrapper TcpTranspondServiceWrapper { get; private set; }
        public TcpClient TcpClient { get; private set; }
        public NetworkStream NetworkStream { get; private set; }

		public TcpTranspondServicePage (TcpTranspondServiceWrapper tcpTranspondServiceWrapper)
		{
			InitializeComponent ();
            TcpTranspondServiceWrapper = tcpTranspondServiceWrapper;
            ReceivedMessages = new ObservableCollection<string>();
            ReceivedMessageListView.ItemsSource = ReceivedMessages;
            TcpClient = new TcpClient();
            TcpTranspondServiceWrapper.StateChanged += TcpTranspondServiceWrapper_StateChanged;
            TcpTranspondServiceWrapper.MessageReceived += TcpTranspondServiceWrapper_MessageReceived;
        }

        private void TcpTranspondServiceWrapper_MessageReceived(object sender, byte[] e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ReceivedMessages.Add(Encoding.UTF8.GetString(e));
            });
        }

        private void TcpTranspondServiceWrapper_StateChanged(object sender, TcpTranspondServiceWrapper.TcpConnState e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if(e == TcpTranspondServiceWrapper.TcpConnState.Created)
                {
                    ConnectButton.IsEnabled = true;
                    ConnectButton.Text = "Connect";
                    SendButton.IsEnabled = false;
                }
                else if(e == TcpTranspondServiceWrapper.TcpConnState.Connecting)
                {
                    ConnectButton.IsEnabled = true;
                    ConnectButton.Text = "Cancel";
                    SendButton.IsEnabled = false;
                }
                else if(e == TcpTranspondServiceWrapper.TcpConnState.Connected)
                {
                    ConnectButton.IsEnabled = false;
                    ConnectButton.Text = "Connected";
                    SendButton.IsEnabled = true;
                }
                else if(e == TcpTranspondServiceWrapper.TcpConnState.Error)
                {
                    ConnectButton.IsEnabled = true;
                    ConnectButton.Text = "Reconnect";
                    SendButton.IsEnabled = false;
                }
            });
        }

        

        private async void ConnectButton_Clicked(object sender, EventArgs e)
        {
            if(TcpTranspondServiceWrapper.State == TcpTranspondServiceWrapper.TcpConnState.Created)
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(IpEditor.Text), int.Parse(PortEditor.Text));
                TcpTranspondServiceWrapper.IPEndPoint = ipEndPoint;
                await TcpTranspondServiceWrapper.ConnectAsync();
            }
            else if(TcpTranspondServiceWrapper.State == TcpTranspondServiceWrapper.TcpConnState.Connecting)
            {
                TcpTranspondServiceWrapper.CancelConnect();
            }
            else if(TcpTranspondServiceWrapper.State == TcpTranspondServiceWrapper.TcpConnState.Error)
            {
                TcpTranspondServiceWrapper.Restore();
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(IpEditor.Text), int.Parse(PortEditor.Text));
                TcpTranspondServiceWrapper.IPEndPoint = ipEndPoint;
                await TcpTranspondServiceWrapper.ConnectAsync();
            }
            
        }

        private async void SendButton_Clicked(object sender, EventArgs e)
        {
            await TcpTranspondServiceWrapper.SendAsync(Encoding.UTF8.GetBytes(SendEditor.Text));
        }

        /*
        private async Task _ReadTask()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (TcpClient.Available > 0)
                    {
                        byte[] buffer = new byte[TcpClient.Available];
                        var size = await NetworkStream.ReadAsync(buffer, 0, TcpClient.Available);
                        var str = Encoding.UTF8.GetString(buffer);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ReceivedMessages.Add(str);
                        });

                    }

                }
            });
            
        }
        */
    }
}