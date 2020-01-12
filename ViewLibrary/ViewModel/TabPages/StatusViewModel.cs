using AlienFXWrapper;
using ChromaFX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //Get settings
            GlobalSettings settings = SettingsManager.GetSettings();

            ObservableCollection<DeviceBaseViewModel> devices = new ObservableCollection<DeviceBaseViewModel>();

            //Check Monitors
            if (!AlienFXLightingControl.FXInit())
            {
                //Error Init, LightFX most likely isn't present
                AlienwareError = true;
                settings.DeviceSettings.HasLightFXSdk = false;
            }
            else
            {
                MonitorDetails[] monitors = AlienFXLightingControl.GetAllDevices();
                AlienFXLightingControl.FXRelease();

                if (monitors != null)
                {
                    foreach (var monitor in monitors)
                    {
                        devices.Add(new MonitorViewModel(monitor));
                    }
                }

                AlienwareError = false;
                settings.DeviceSettings.HasLightFXSdk = true;
            }

            //Check Speakers
            bool isChromaInitialized = false;
            try
            {
                isChromaInitialized = Chroma.Instance.Initialized;
            }
            catch (Exception)
            {
                //Error init, Chroma SDK most likely isn't present
                isChromaInitialized = false;
                ChromaError = true;
                settings.DeviceSettings.HasNommo = false;
                settings.DeviceSettings.HasNommoPro = false;
            }

            settings.DeviceSettings.HasChromaSDK = isChromaInitialized;

            if (isChromaInitialized)
            {
                //Check for Nommo
                ChromaFX.Devices.DeviceInfo speaker;
                try
                {
                    speaker = Chroma.Instance.Query(ChromaFX.Devices.Devices.Nommo);
                    devices.Add(new SpeakerViewModel(speaker)
                    {
                        DeviceName = "Razer Nommo"
                    });

                    settings.DeviceSettings.HasNommo = true;

                }
                catch (Exception)
                {
                    //No device present
                    settings.DeviceSettings.HasNommo = false;
                }

                //Check for Nommo Pro
                try
                {
                    speaker = Chroma.Instance.Query(ChromaFX.Devices.Devices.NommoPro);
                    devices.Add(new SpeakerViewModel(speaker)
                    {
                        DeviceName = "Razer Nommo Pro"
                    });
                    settings.DeviceSettings.HasNommoPro = true;
                }
                catch (Exception)
                {
                    //No device present
                    settings.DeviceSettings.HasNommoPro = false;
                }

                ChromaError = false;

                Chroma.Instance.Uninitialize();
            }

            Devices = devices;

            SettingsManager.SaveSettings(settings);
        }
    }
}
