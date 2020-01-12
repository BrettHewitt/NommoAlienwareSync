using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
