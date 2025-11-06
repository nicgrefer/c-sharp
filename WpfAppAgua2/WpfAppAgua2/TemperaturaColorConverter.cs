using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfAppAgua
{
    internal class TemperaturaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
                if ((int)value <= 10)
                    return "Blue";
                else if ((int)value > 10 && (int)value <= 20)
                    return "Green";
                else
                    return "Red";




        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
