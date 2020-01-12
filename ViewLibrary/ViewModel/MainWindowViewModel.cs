using AlienFXWrapper;
using ChromaFX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.ViewModel.Devices;
using ViewLibrary.ViewModel.TabPages;

namespace ViewLibrary.ViewModel
{
    public class MainWindowViewModel : WindowViewModelBase
    {
        private StatusViewModel _StatusVm;
        public StatusViewModel StatusVm
        {
            get
            {
                return _StatusVm;
            }
            set
            {
                if (Equals(_StatusVm, value))
                {
                    return;
                }

                _StatusVm = value;

                NotifyPropertyChanged();
            }
        }

        private LightingViewModel _LightingVm;
        public LightingViewModel LightingVm
        {
            get
            {
                return _LightingVm;
            }
            set
            {
                if (Equals(_LightingVm, value))
                {
                    return;
                }

                _LightingVm = value;

                NotifyPropertyChanged();
            }
        }

        private SettingsViewModel _SettingsVm;
        public SettingsViewModel SettingsVm
        {
            get
            {
                return _SettingsVm;
            }
            set
            {
                if (Equals(_SettingsVm, value))
                {
                    return;
                }

                _SettingsVm = value;

                NotifyPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            StatusVm = new StatusViewModel();
            LightingVm = new LightingViewModel();
            SettingsVm = new SettingsViewModel();
        }
    }
}
