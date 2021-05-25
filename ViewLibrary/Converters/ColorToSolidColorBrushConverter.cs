using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ViewLibrary.Extensions;

namespace ViewLibrary.Converters
{
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color color;
            if (value.GetType() == typeof(System.Drawing.Color))
            {
                color = ((System.Drawing.Color)value).ToMediaColor();
            }
            else
            {
                color = (Color)value;
            }

            if (value != null)
                return new SolidColorBrush(color);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return ((SolidColorBrush)value).Color;

            return value;
        }
    }
}
