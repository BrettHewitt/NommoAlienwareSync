using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.ViewModel.TabPages.Settings
{
    public abstract class SettingsBaseViewModel : ViewModelBase
    {
        private string _SettingName;
        public string SettingName
        {
            get
            {
                return _SettingName;
            }
            set
            {
                if (Equals(_SettingName, value))
                {
                    return;
                }

                _SettingName = value;

                NotifyPropertyChanged();
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (Equals(_Description, value))
                {
                    return;
                }

                _Description = value;

                NotifyPropertyChanged();
            }
        }

        protected SettingsBaseViewModel(string name)
        {
            SettingName = name;
            Description = SetDescription();
            LoadSettings();
        }

        protected abstract string SetDescription();
        protected abstract void LoadSettings();
        public abstract void SaveSettings();
    }
}
