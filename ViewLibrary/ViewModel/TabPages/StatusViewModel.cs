using AlienFXWrapper;
using ChromaFX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Model.Effects;
using ViewLibrary.Model.Settings;
using ViewLibrary.Properties;
using ViewLibrary.ViewModel.Devices;

namespace ViewLibrary.ViewModel.TabPages
{
    public class StatusViewModel : ViewModelBase
    {
        private ObservableCollection<DeviceBaseViewModel> _Devices;
        public ObservableCollection<DeviceBaseViewModel> Devices
        {
            get
            {
                return _Devices;
            }
            set
            {
                if (Equals(_Devices, value))
                {
                    return;
                }

                _Devices = value;

                NotifyPropertyChanged();
            }
        }


        private bool _AlienwareError;
        public bool AlienwareError
        {
            get
            {
                return _AlienwareError;
            }
            set
            {
                if (Equals(_AlienwareError, value))
                {
                    return;
                }

                _AlienwareError = value;

                NotifyPropertyChanged();
            }
        }

        private bool _ChromaError;
        public bool ChromaError
        {
            get
            {
                return _ChromaError;
            }
            set
            {
                if (Equals(_ChromaError, value))
                {
                    return;
                }

                _ChromaError = value;

                NotifyPropertyChanged();
            }
        }


        public StatusViewModel()
        {
            LoadDevices();
        }

        private void LoadDevices()
        {
            EffectManager.Instance.DeviceChanged += Instance_DeviceChanged;
            EffectManager.Instance.Initialise();
        }

        private void Instance_DeviceChanged(object sender, DeviceChangedEventArgs args)
        {
            //Get settings
            GlobalSettings settings = SettingsManager.GetSettings();
            ObservableCollection<DeviceBaseViewModel> devices = new ObservableCollection<DeviceBaseViewModel>();

            settings.DeviceSettings.HasLightFXSdk = args.HasLightFX;
            AlienwareError = !args.HasLightFX;

            if (args.HasLightFX)
            {
                MonitorDetails[] monitors = AlienFXLightingControl.GetAllDevices();

                if (monitors != null)
                {
                    foreach (var monitor in monitors)
                    {
                        devices.Add(new MonitorViewModel(monitor));
                    }
                }
            }

            settings.DeviceSettings.HasChromaSDK = args.HasChroma;
            ChromaError = !args.HasChroma;

            if (args.HasChroma)
            {
                if (args.ChromaGuid == ChromaFX.Devices.Devices.Nommo && args.ChromaSpeaker.HasValue)
                {
                    devices.Add(new SpeakerViewModel(args.ChromaSpeaker.Value)
                    {
                        DeviceName = "Razer Nommo"
                    });

                    settings.DeviceSettings.HasNommo = true;
                }
                else if (args.ChromaGuid == ChromaFX.Devices.Devices.NommoPro && args.ChromaSpeaker.HasValue)
                {
                    devices.Add(new SpeakerViewModel(args.ChromaSpeaker.Value)
                    {
                        DeviceName = "Razer Nommo Pro"
                    });

                    settings.DeviceSettings.HasNommoPro = true;
                }
            }

            Devices = devices;

            SettingsManager.SaveSettings(settings);

            EffectManager.Instance.ResetEffect();
        }
    }
}
