using AlienFXWrapper;
using ChromaFX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Commands;
using ViewLibrary.Model.Effects;
using ViewLibrary.Model.Settings;
using ViewLibrary.Properties;
using ViewLibrary.ViewModel.Devices;

namespace ViewLibrary.ViewModel.TabPages
{
    public class StatusViewModel : ViewModelBase
    {
        private ActionCommand _RefreshCommand;
        public ActionCommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new ActionCommand()
                {
                    ExecuteAction = () => EffectManager.Instance.Initialise()
                });
            }
        }

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


        private bool _AlienwareError = true;
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

        private bool _ChromaError = true;
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
            EffectManager.Instance.MonitorDeviceChanged += OnMonitorDeviceChanged;
            EffectManager.Instance.Initialise();
        }

        private void OnMonitorDeviceChanged(object sender, DeviceChangedEventArgs args)
        {
            ObservableCollection<DeviceBaseViewModel> devices = new ObservableCollection<DeviceBaseViewModel>();

            RuntimeGlobals.HasLightFXSdk = args.HasLightFX;

            AlienwareError = !args.HasLightFX;

            PopulateMonitors(devices);
            PopulateSpeakers(devices);

            Devices = devices;

            EffectManager.Instance.ResetEffect();
        }

        private void PopulateMonitors(ObservableCollection<DeviceBaseViewModel> devices)
        {
            bool hasLightFx = RuntimeGlobals.HasLightFXSdk;
            if (hasLightFx)
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
        }

        private void PopulateSpeakers(ObservableCollection<DeviceBaseViewModel> devices)
        {
            bool hasChroma = RuntimeGlobals.HasChromaSDK;
            if (hasChroma)
            {
                if (RuntimeGlobals.HasNommo)
                {
                    devices.Add(new SpeakerViewModel()
                    {
                        DeviceName = "Razer Nommo"
                    });
                }
                else if (RuntimeGlobals.HasNommoPro)
                {
                    devices.Add(new SpeakerViewModel()
                    {
                        DeviceName = "Razer Nommo Pro"
                    });
                }
            }
        }

        private void Instance_DeviceChanged(object sender, DeviceChangedEventArgs args)
        {
            ObservableCollection<DeviceBaseViewModel> devices = new ObservableCollection<DeviceBaseViewModel>();
            
            RuntimeGlobals.HasChromaSDK = args.HasChroma;

            if (args.HasChroma)
            {
                if (args.ChromaGuid == ChromaFX.Devices.Devices.Nommo && args.ChromaSpeaker.HasValue)
                {
                    RuntimeGlobals.HasNommo = true;
                }
                else if (args.ChromaGuid == ChromaFX.Devices.Devices.NommoPro && args.ChromaSpeaker.HasValue)
                {
                    RuntimeGlobals.HasNommoPro = true;
                }
            }

            ChromaError = !args.HasChroma;

            PopulateMonitors(devices);
            PopulateSpeakers(devices);

            Devices = devices;
            
            EffectManager.Instance.ResetEffect();
        }
    }
}
