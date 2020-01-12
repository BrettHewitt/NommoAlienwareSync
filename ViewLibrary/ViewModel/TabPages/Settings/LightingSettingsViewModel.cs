using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewLibrary.Model.Settings;

namespace ViewLibrary.ViewModel.TabPages.Settings
{
    public class LightingSettingsViewModel : SettingsBaseViewModel
    {
        private int _RefreshRate;
        public int RefreshRate
        {
            get
            {
                return _RefreshRate;
            }
            set
            {
                if (Equals(_RefreshRate, value))
                {
                    return;
                }

                _RefreshRate = value;

                NotifyPropertyChanged();
            }
        }
        
        public LightingSettingsViewModel() : base("Lighting Settings")
        {

        }

        protected override string SetDescription()
        {
            return "Lighting operation settings";
        }

        protected override void LoadSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            RefreshRate = settings.ProgramSettings.LightingSettings.RefreshRate;
        }

        public override void SaveSettings()
        {
            if (RefreshRate < 10 || RefreshRate > 1000)
            {
                MessageBox.Show("Refresh Rate must be between 10 and 1000");
                return;
            }

            GlobalSettings settings = SettingsManager.GetSettings();
            settings.ProgramSettings.LightingSettings.RefreshRate = RefreshRate;
            SettingsManager.SaveSettings(settings);
        }
    }
}
