﻿using RemoteX.Bluetooth;
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
	public partial class LEDevicePage : ContentView
	{
        IBluetoothDevice BluetoothDevice { get; }
        
		public LEDevicePage (IBluetoothDevice bluetoothDevice)
		{
            BluetoothDevice = bluetoothDevice;
			InitializeComponent ();
		}
	}
}