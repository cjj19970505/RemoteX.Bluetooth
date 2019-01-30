using RemoteX.Bluetooth.LE.Gatt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.GattServer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GattRequestViewCell : ViewCell
	{
        public GattRequestViewModel GattRequestViewModel { get; private set; }

		public GattRequestViewCell ()
		{
			InitializeComponent ();
            BindingContextChanged += _OnBindingContextChanged;
            ResponseErrorCodePicker.SelectedIndex = 0;


        }

        private void _OnBindingContextChanged(object sender, EventArgs e)
        {
            GattRequestViewModel = BindingContext as GattRequestViewModel;
        }

        private void RespondButton_Clicked(object sender, EventArgs e)
        {
            if(GattRequestViewModel.GattServerRequest is ICharacteristicReadRequest)
            {
                if(ResponseErrorCodePicker.SelectedIndex == 0)
                {
                    (GattRequestViewModel.GattServerRequest as ICharacteristicReadRequest).RespondWithValue(Encoding.UTF8.GetBytes(ResponseEditor.Text));
                }
                else
                {
                    GattErrorCode errorcode = (GattErrorCode)(Enum.Parse(typeof(GattErrorCode), GattRequestViewModel.ErrorCodeNames[ResponseErrorCodePicker.SelectedIndex]));
                    GattRequestViewModel.GattServerRequest.RespondWithProtocolError(errorcode);
                }
                
            }
            else if (GattRequestViewModel.GattServerRequest is ICharacteristicWriteRequest)
            {
                if(ResponseErrorCodePicker.SelectedIndex == 0)
                {
                    (GattRequestViewModel.GattServerRequest as ICharacteristicWriteRequest).RespondSuccess();
                }
                else
                {
                    GattErrorCode errorcode = (GattErrorCode)(Enum.Parse(typeof(GattErrorCode), GattRequestViewModel.ErrorCodeNames[ResponseErrorCodePicker.SelectedIndex]));
                    GattRequestViewModel.GattServerRequest.RespondWithProtocolError(errorcode);
                }
            }
        }
    }
}