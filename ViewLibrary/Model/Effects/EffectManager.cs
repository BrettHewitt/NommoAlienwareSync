using AlienFXWrapper;
using ChromaFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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
            bool hasChromaSdk = InitChroma();
            Guid chromaGuid = Guid.Empty;
            ChromaFX.Devices.DeviceInfo? speaker = null;
            if (hasChromaSdk)
            {
                //Check for Nommo
                try
                {
                    speaker = Chroma.Instance.Query(ChromaFX.Devices.Devices.Nommo);
                    chromaGuid = ChromaFX.Devices.Devices.Nommo;
                }
                catch (Exception)
                {
                    //No device present
                }

                //Check for Nommo Pro
                try
                {
                    speaker = Chroma.Instance.Query(ChromaFX.Devices.Devices.NommoPro);
                    chromaGuid = ChromaFX.Devices.Devices.NommoPro;
                }
                catch (Exception)
                {
                    //No device presents
                }
            }

            bool hasLightFX = InitLightFX();

            if (DeviceChanged != null)
            {
                DeviceChanged(this, new DeviceChangedEventArgs()
                {
                    HasChroma = hasChromaSdk,
                    HasLightFX = hasLightFX,
                    ChromaGuid = chromaGuid,
                    ChromaSpeaker = speaker,
                });
            }


            if (!hasLightFX)
            {
                //Light FX isn't initalised, but could be because we are waiting for the service to start running
                //Check if service exists and if so wait for it to start running
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        using (ServiceController sc = new ServiceController("AWCCService"))
                        {
                            sc.WaitForStatus(ServiceControllerStatus.Running);
                            Thread.Sleep(3000);
                            Dispatcher.Invoke(() =>
                            {
                                hasLightFX = InitLightFX();

                                DeviceChanged(this, new DeviceChangedEventArgs()
                                {
                                    HasChroma = hasChromaSdk,
                                    HasLightFX = hasLightFX,
                                    ChromaGuid = chromaGuid,
                                    ChromaSpeaker = speaker,
                                });
                            });
                        }
                    }
                    catch (Exception)
                    {
                        //Service doesn't exist
                        return;
                    }
                });
            }
        }

        public void Uninitialise()
        {
            AlienFXLightingControl.FXRelease();
            Chroma.Instance.Uninitialize();
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
