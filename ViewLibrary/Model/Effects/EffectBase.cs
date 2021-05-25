using System;
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

        private readonly object _HasHueLock = new object();
        private bool _HasHue;
        public bool HasHue
        {
            get
            {
                lock (_HasHueLock)
                {
                    return _HasHue;
                }
            }
            set
            {
                lock (_HasHueLock)
                {
                    _HasHue = value;
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
        private int _RefreshRate = 50;
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
        }

        private void UpdateSettings()
        {
            HasHue = RuntimeGlobals.HasHue;
            HasLightFX = RuntimeGlobals.HasLightFXSdk;
            HasChroma = RuntimeGlobals.HasChromaSDK;
            //ChromaGuid = RuntimeGlobals.HasNommo ? ChromaFX.Devices.Devices.Nommo : ChromaFX.Devices.Devices.NommoPro;
        }

        public void StartEffect()
        {
            UpdateSettings();
            SaveSettings();
            EffectManager.Instance.StartEffect(this);
        }

        public void StopEffect()
        {
            EffectManager.Instance.StopEffect();
        }

        protected abstract string SetDescription();
        public abstract void EffectAction(); 

        

        protected abstract void LoadSettings();
        public abstract void SaveSettings();
    }
}
