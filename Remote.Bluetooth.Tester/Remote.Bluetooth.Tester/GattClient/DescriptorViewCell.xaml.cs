using RemoteX.Bluetooth.LE.Gatt.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.GattClient
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DescriptorViewCell : ViewCell
	{
        public IGattClientDescriptor Descriptor
        {
            get
            {
                return BindingContext as IGattClientDescriptor;
            }
        }
		public DescriptorViewCell ()
		{
			InitializeComponent ();
		}

        private async void WriteButton_Clicked(object sender, EventArgs e)
        {
            //WriteButton.IsEnabled = false;
            Descriptor.WriteAsync(new byte[1] { 1 });
            
            //SendStateLabel.Text = result.ToString();
            //WriteButton.IsEnabled = true;
        }
    }
}