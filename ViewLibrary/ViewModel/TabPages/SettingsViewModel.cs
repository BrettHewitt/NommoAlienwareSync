using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Commands;
using ViewLibrary.ViewModel.TabPages.Settings;

namespace ViewLibrary.ViewModel.TabPages
{
    public class SettingsViewModel : ViewModelBase
    {
        private ActionCommand _ApplyCommand;
        public ActionCommand ApplyCommand
        {
            get
            {
                return _ApplyCommand ?? (_ApplyCommand = new ActionCommand()
                {
                    ExecuteAction = ApplySettings
                });
            }
        }

        private ObservableCollection<SettingsBaseViewModel> _Settings;
        public ObservableCollection<SettingsBaseViewModel> Settings
        {
            get
            {
                return _Settings;
            }
            set
            {
                if (Equals(_Settings, value))
                {
                    return;
                }

                _Settings = value;

                NotifyPropertyChanged();
            }
        }

        private SettingsBaseViewModel _SelectedSetting;
        public SettingsBaseViewModel SelectedSetting
        {
            get
            {
                return _SelectedSetting;
            }
            set
            {
                if (Equals(_SelectedSetting, value))
                {
                    return;
                }

                _SelectedSetting = value;

                NotifyPropertyChanged();
            }
        }

        public SettingsViewModel()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            ObservableCollection<SettingsBaseViewModel> settings = new ObservableCollection<SettingsBaseViewModel>();
            settings.Add(new GeneralSettingsViewModel());
            settings.Add(new LightingSettingsViewModel());
            Settings = settings;
            
            SelectedSetting = Settings.First();
        }

        private void ApplySettings()
        {
            foreach (SettingsBaseViewModel setting in Settings)
            {
                setting.SaveSettings();
            }
        }
    }
}
