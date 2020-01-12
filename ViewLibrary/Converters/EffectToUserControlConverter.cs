using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ViewLibrary.View.Effects;
using ViewLibrary.ViewModel.TabPages.Effects;

namespace ViewLibrary.Converters
{
    public class EffectToUserControlConverter : IValueConverter
    {
        private SpectrumUserControl _Spectrum = new SpectrumUserControl();
        private ScreenCaptureUserControl _ScreenCapture = new ScreenCaptureUserControl();
        private CustomUserControl _Custom = new CustomUserControl();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SpectrumViewModel spectrum = value as SpectrumViewModel;
            if (spectrum != null)
            {
                _Spectrum.DataContext = spectrum;
                return _Spectrum; 
            }

            ScreenCaptureViewModel screenCapture = value as ScreenCaptureViewModel;
            if (screenCapture != null)
            {
                _ScreenCapture.DataContext = screenCapture;
                return _ScreenCapture;
            }

            CustomEffectViewModel custom = value as CustomEffectViewModel;
            if (custom != null)
            {
                _Custom.DataContext = custom;
                return _Custom;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
