using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ViewLibrary.View.Settings;
using ViewLibrary.ViewModel.TabPages.Settings;

namespace ViewLibrary.Converters
{
    public class SettingToUserControlConverter : IValueConverter
    {
        private GeneralSettingsView _General = new GeneralSettingsView();
        private LightingSettingsView _Lighting = new LightingSettingsView();
        private HueSettingsView _Hue = new HueSettingsView();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GeneralSettingsViewModel general)
            {
                _General.DataContext = general;
                return _General;
            }

            if (value is LightingSettingsViewModel lighting)
            {
                _Lighting.DataContext = lighting;
                return _Lighting;
            }

            if (value is HueSettingsViewModel hue)
            {
                _Hue.DataContext = hue;
                return _Hue;
            }

            return null;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
