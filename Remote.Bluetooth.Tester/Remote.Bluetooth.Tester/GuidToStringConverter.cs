using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Remote.Bluetooth.Tester
{
    public class GuidToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Guid)value).ToString();
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Guid result;
            if(!Guid.TryParse(value as string, out result))
            {
                return Guid.Empty;
            }
            return result;
        }
    }
}
