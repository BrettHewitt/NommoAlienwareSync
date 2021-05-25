using AlienFXWrapper;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ChromaFX;
using ViewLibrary.Helpers;
using ViewLibrary.Model.Hue;
using ViewLibrary.Model.Settings;

namespace ViewLibrary.Model.Effects
{
    public class EffectManager
    {
        public static EffectManager Instance { get; } = new EffectManager();
        private EffectManager()
        {

        }

        private EffectBase CurrentEffect
        {
            get;
            set;
        }

        private Task CurrentTask
        {
            get;
            set;
        }

        private bool LightFXInitialised
        {
            get;
            set;
        }

        private bool ChromaSDKInitialised
        {
            get;
            set;
        }

        private Dispatcher Dispatcher
        {
            get;
            set;
        }

        public event DeviceChangedHandler DeviceChanged;
        public event DeviceChangedHandler MonitorDeviceChanged;

        public void Exiting()
        {
            HueFX.Instance.ShutDown();
        }

        public void StartEffect(EffectBase effect)
        {
            if (CurrentEffect != null)
            {
                CurrentEffect.StopEffect();
            }

            effect.RefreshRate = SettingsManager.GetSettings().ProgramSettings.LightingSettings.RefreshRate;
            CurrentEffect = effect;
            CurrentTask = Task.Factory.StartNew(effect.EffectAction);
        }

        public void StopEffect()
        {
            if (CurrentEffect == null || CurrentTask == null)
            {
                return;
            }

            CurrentEffect.UseEffect = false;

            try
            {
                CurrentTask.Wait();
            }
            catch (ObjectDisposedException)
            {

            }

            CurrentEffect = null;
            CurrentTask = null;
        }

        public void ResetEffect()
        {
            if (CurrentEffect == null || CurrentTask == null)
            {
                return;
            }

            EffectBase currentEffect = CurrentEffect;
            currentEffect.StopEffect();
            currentEffect.StartEffect();
        }

        public void Initialise()
        {
            Task.Factory.StartNew(StartHueDetection);
            Task.Factory.StartNew(StartChromaDetection);
            Task.Factory.StartNew(StartLightFXDetection);
        }

        private void StartHueDetection()
        {
            var settings = SettingsManager.GetSettings();

            if (!settings.HueSettings.EnableHue)
            {
                RuntimeGlobals.HasHue = false;
                return;
            }

            string key = settings.HueSettings.Key;

            if (string.IsNullOrWhiteSpace(key))
            {
                //Hue not configured
                RuntimeGlobals.HasHue = false;
                return;
            }

            HueFX.Instance.InitHueEdk(key);

            HueFX.Instance.BridgeConnected += OnBridgeConnected;
            HueFX.Instance.UserProcedureFinished += OnUserProcedureFinished;
            
            HueFX.Instance.Connect();
        }

        private void OnBridgeConnected(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                DeviceChanged?.Invoke(this, new DeviceChangedEventArgs()
                {
                    HasHue = true,
                });
            });

            HueFX.Instance.BridgeConnected -= OnBridgeConnected;
        }

        private void OnUserProcedureFinished(object sender, EventArgs e)
        {
            RuntimeGlobals.HasHue = false;
            HueFX.Instance.UserProcedureFinished -= OnUserProcedureFinished;
        }

        private void StartChromaDetection()
        {
            bool hasChromaSdk = InitChroma();
            
            if (!hasChromaSdk)
            {
                return;
            }
            
            Dispatcher.Invoke(() =>
            {
                DeviceChanged?.Invoke(this, new DeviceChangedEventArgs()
                {
                    HasChroma = true,
                });
            });
        }

        private void StartLightFXDetection()
        {
            //Light FX isn't initalised, but could be because we are waiting for the service to start running
            //Check if service exists and if so wait for it to start running                
            try
            {
                using (ServiceController sc = new ServiceController("AWCCService"))
                {
                    while (true)
                    {                        
                        sc.WaitForStatus(ServiceControllerStatus.Running);
                        Thread.Sleep(5000);

                        bool hasLightFX = InitLightFX();
                        if (hasLightFX)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                MonitorDeviceChanged?.Invoke(this, new DeviceChangedEventArgs()
                                {
                                    HasLightFX = hasLightFX,
                                });
                            });

                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Service doesn't exist
                return;
            }
        }

        public void Uninitialise()
        {
            UnInitChroma();
            HueFX.Instance.ShutDown();
            UnInitLightFX();
        }

        private bool InitLightFX()
        {
            return AlienFXLightingControl.FXInit();
        }

        private bool InitChroma()
        {
            try
            {
                Chroma.Instance.Initialize();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool UnInitLightFX()
        {
            return AlienFXLightingControl.FXRelease();
        }

        private bool UnInitChroma()
        {
            try
            {
                Chroma.Instance.Uninitialize();
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        public void SetDispatcher(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }
    }
}
