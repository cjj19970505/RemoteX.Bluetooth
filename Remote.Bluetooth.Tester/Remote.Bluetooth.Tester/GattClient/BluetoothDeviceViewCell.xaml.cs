﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.GattClient
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BluetoothDeviceViewCell : ViewCell
	{
        
		public BluetoothDeviceViewCell ()
		{
			InitializeComponent ();
		}
	}
}