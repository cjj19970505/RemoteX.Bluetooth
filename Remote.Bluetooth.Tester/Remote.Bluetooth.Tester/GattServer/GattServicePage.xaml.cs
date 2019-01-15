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
	public partial class GattServicePage : ContentPage
	{
        public GattServiceModel GattServiceModel { get; }
		public GattServicePage (GattServiceModel gattServiceModel)
		{
            GattServiceModel = gattServiceModel;
            BindingContext = GattServiceModel;
            InitializeComponent ();
		}
	}
}