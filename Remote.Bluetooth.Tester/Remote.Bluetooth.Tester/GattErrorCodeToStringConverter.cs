using RemoteX.Bluetooth.LE.Gatt;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Remote.Bluetooth.Tester
{
    class GattErrorCodeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.GetName(GattErrorCode.ConnectionCongested.GetType(), (GattErrorCode)value);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.Parse(GattErrorCode.ConnectionCongested.GetType(), value.ToString());
            
        }
    }
}
