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
	public partial class GattCharacteristicViewCell : ViewCell
	{
        IGattClientCharacteristic Characteristic
        {
            get
            {
                return BindingContext as IGattClientCharacteristic;
            }
        }

		public GattCharacteristicViewCell ()
		{
            InitializeComponent ();
		}

        protected override void OnBindingContextChanged()
        {
            if(Characteristic != null)
            {
                PropertiesLabel.Text = Characteristic.CharacteristicProperties.ToString();
            }
            
        }

        private async void GetValueButton_Clicked(object sender, EventArgs e)
        {
            var result = await Characteristic.ReadCharacteristicValueAsync();
            if (result.ProtocolError == GattErrorCode.Success)
            {
                CharacteristicValueLabel.Text = Encoding.UTF8.GetString(result.Value);
            }
            else
            {
                CharacteristicValueLabel.Text = Enum.GetName(typeof(GattErrorCode), result.ProtocolError);
            }
        }



    }
}