using RemoteX.Bluetooth;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Remote.Bluetooth.Tester
{
    public class MacAddressUlongToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return BluetoothUtils.AddressInt64ToString((ulong)value);
            }
            catch (Exception)
            {
                return "Unknown Address";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return BluetoothUtils.AddressStringToInt64(targetType.ToString());
            }
            catch (Exception)
            {
                return 0ul;
            }
           
        }
    }
}
