using AlienFXWrapper;
using ChromaFX;
using ChromaFX.Devices.Speakers;
using ChromaFX.DevicesInterface.Speakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViewLibrary.Model.Settings;

namespace ViewLibrary.Model.Effects
{
    public class Spectrum : TempoBaseEffect
    {
        public Spectrum() : base ("Spectrum")
        {

        }

        protected override string SetDescription()
        {
            return "Cycle through the spectrum at a custom speed";
        }

        protected override void LoadSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            Tempo = settings.SpectrumSettings.Tempo;
        }

        public override void SaveSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            settings.SpectrumSettings.Tempo = Tempo;
            SettingsManager.SaveSettings(settings);
        }

        public override void EffectAction()
        {
            SaveSettings();
            UseEffect = true;
            ISpeakers speaker = new Speakers();

            double timer = 0;
            int refreshRate = RefreshRate;
            double timerLimit = 100000 / Tempo;
            double tempo = Tempo;
            Initialise();

            while (UseEffect)
            {
                Color color = GetSpectrumColor((int)((timer * tempo) / 1000));

                if (HasLightFX)
                {
                    AlienFXLightingControl.SetFXColour(color.R, color.G, color.B);
                }

                if (HasChroma && ChromaGuid != Guid.Empty)
                {
                    Custom custom = new Custom(color);
                    speaker.SetStatic(custom, ChromaGuid);
                }

                Thread.Sleep(refreshRate);
                timer += refreshRate;

                if (timer > timerLimit)
                {
                    timer -= timerLimit;
                }
            }

            Uninitialise();
        }

        private static Color GetSpectrumColor(int w)
        {
            float r = 0.0f;
            float g = 0.0f;
            float b = 0.0f;

            w = w % 100;

            if (w < 17)
            {
                r = -(w - 17.0f) / 17.0f;
                b = 1.0f;
            }
            else if (w < 33)
            {
                g = (w - 17.0f) / (33.0f - 17.0f);
                b = 1.0f;
            }
            else if (w < 50)
            {
                g = 1.0f;
                b = -(w - 50.0f) / (50.0f - 33.0f);
            }
            else if (w < 67)
            {
                r = (w - 50.0f) / (67.0f - 50.0f);
                g = 1.0f;
            }
            else if (w < 83)
            {
                r = 1.0f;
                g = -(w - 83.0f) / (83.0f - 67.0f);
            }
            else
            {
                r = 1.0f;
                b = (w - 83.0f) / (100.0f - 83.0f);
            }

            return new Color((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }
    }
}
