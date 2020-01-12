using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Model.Settings;

namespace ViewLibrary.ViewModel.TabPages.Settings
{
    public class GeneralSettingsViewModel : SettingsBaseViewModel
    {
        private bool _StartOnStartup;
        public bool StartOnStartup
        {
            get
            {
                return _StartOnStartup;
            }
            set
            {
                if (Equals(_StartOnStartup, value))
                {
                    return;
                }

                _StartOnStartup = value;

                NotifyPropertyChanged();
            }
        }

        private bool _StartEffectOnStartup;
        public bool StartEffectOnStartup
        {
            get
            {
                return _StartEffectOnStartup;
            }
            set
            {
                if (Equals(_StartEffectOnStartup, value))
                {
                    return;
                }

                _StartEffectOnStartup = value;

                NotifyPropertyChanged();
            }
        }
        
        public GeneralSettingsViewModel() : base("General Settings")
        {

        }

        protected override string SetDescription()
        {
            return "General settings for program operation";
        }

        protected override void LoadSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            StartOnStartup = settings.ProgramSettings.GeneralSettings.StartOnStartup;
            StartEffectOnStartup = settings.ProgramSettings.GeneralSettings.StartEffectOnStartup;
        }

        public override void SaveSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            settings.ProgramSettings.GeneralSettings.StartOnStartup = StartOnStartup;
            settings.ProgramSettings.GeneralSettings.StartEffectOnStartup = StartEffectOnStartup;
            SettingsManager.SaveSettings(settings);
            SetStartup(StartOnStartup);
        }

        private void SetStartup(bool startUp)
        {
            string appName = "dataDyne AC Lighting";
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(directory, "ACLightingControl.exe");

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (startUp)
            {
                rk.SetValue(appName, fullPath);
            }
            else
            {
                rk.DeleteValue(appName, false);
            }
        }
    }
}
