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

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GeneralSettingsViewModel general = value as GeneralSettingsViewModel;
            if (general != null)
            {
                _General.DataContext = general;
                return _General;
            }

            LightingSettingsViewModel lighting = value as LightingSettingsViewModel;
            if (lighting != null)
            {
                _Lighting.DataContext = lighting;
                return _Lighting;
            }

            return null;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
