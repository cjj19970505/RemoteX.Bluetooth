using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using RemoteX.Bluetooth;
using RemoteX.Droid.Bluetooth;
using RemoteX.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Remote.Bluetooth.Tester.Droid.ManagerManagerFragment))]
namespace Remote.Bluetooth.Tester.Droid
{
    public class ManagerManagerFragment : Fragment, IManagerManager
    {
        private BluetoothManager _BluetoothManager;
        public IBluetoothManager BluetoothManager
        {
            get
            {
                if(_BluetoothManager == null)
                {
                    _BluetoothManager = new BluetoothManager();
                }
                return _BluetoothManager;
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
        public override void OnStart()
        {
            base.OnStart();
            System.Diagnostics.Debug.WriteLine("XJ:::FRAGMENTSTART");
        }
    }
}