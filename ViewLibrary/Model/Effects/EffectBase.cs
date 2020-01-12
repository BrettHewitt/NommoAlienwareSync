using AlienFXWrapper;
using ChromaFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Model.Settings;

namespace ViewLibrary.Model.Effects
{
    public abstract class EffectBase
    {
        public string EffectName
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        private object ThreadLock = new object();
        private bool _UseEffect;
        public bool UseEffect
        {
            get
            {
                lock (ThreadLock)
                {
                    return _UseEffect;
                }
            }
            set
            {
                lock (ThreadLock)
                {
                    _UseEffect = value;
                }
            }
        }

        private readonly object _HasLightFXLock = new object();
        private bool _HasLightFX;
        public bool HasLightFX
        {
            get
            {
                lock (_HasLightFXLock)
                {
                    return _HasLightFX;
                }
            }
            set
            {
                lock (_HasLightFXLock)
                {
                    _HasLightFX = value;
                }
            }
        }

        private readonly object _HasChromaLock = new object();
        private bool _HasChroma;
        public bool HasChroma
        {
            get
            {
                lock (_HasChromaLock)
                {
                    return _HasChroma;
                }
            }
            set
            {
                lock (_HasChromaLock)
                {
                    _HasChroma = value;
                }
            }
        }

        private readonly object _ChromaGuidLock = new object();
        private Guid _ChromaGuid;
        public Guid ChromaGuid
        {
            get
            {
                lock (_ChromaGuidLock)
                {
                    return _ChromaGuid;
                }
            }
            set
            {
                lock (_ChromaGuidLock)
                {
                    _ChromaGuid = value;
                }
            }
        }

        private readonly object _RefreshRateLock = new object();
        private int _RefreshRate = 20;
        public int RefreshRate
        {
            get
            {
                lock (_RefreshRateLock)
                {
                    return _RefreshRate;
                }
            }
            set
            {
                lock (_RefreshRateLock)
                {
                    _RefreshRate = value;
                }
            }
        }

        protected EffectBase(string effectName)
        {
            EffectName = effectName;
            Description = SetDescription();
            LoadSettings();
            GlobalSettings settings = SettingsManager.GetSettings();
            HasLightFX = settings.DeviceSettings.HasLightFXSdk;
            HasChroma = settings.DeviceSettings.HasChromaSDK;
            ChromaGuid = settings.DeviceSettings.HasNommo ? ChromaFX.Devices.Devices.Nommo : ChromaFX.Devices.Devices.NommoPro;
        }

        public void StartEffect()
        {
            EffectManager.Instance.StartEffect(this);
        }

        public void StopEffect()
        {
            EffectManager.Instance.StopEffect();
        }

        protected abstract string SetDescription();
        public abstract void EffectAction(); 

        protected void Initialise()
        {
            if (HasLightFX)
            {
                AlienFXLightingControl.FXInit();
            }

            if (HasChroma && ChromaGuid != Guid.Empty)
            {
                Chroma.Instance.Initialize();
            }
        }

        protected void Uninitialise()
        {
            if (HasLightFX)
            {
                AlienFXLightingControl.FXRelease();
            }

            if (HasChroma && ChromaGuid != Guid.Empty)
            {
                Chroma.Instance.Uninitialize();
            }
        }

        protected abstract void LoadSettings();
        public abstract void SaveSettings();
    }
}
