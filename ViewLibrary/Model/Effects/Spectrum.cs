using AlienFXWrapper;
using System;
using System.Threading;
using ChromaFX;
using ChromaFX.Devices.Speakers;
using ChromaFX.DevicesInterface.Speakers;
using ViewLibrary.Extensions;
using ViewLibrary.Model.Hue;
using ViewLibrary.Model.Settings;
using Color = System.Drawing.Color;

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
            UseEffect = true;
            ISpeakers speakers = new Speakers();
            double timer = 0;
            int refreshRate = RefreshRate;
            double timerLimit = 100000 / Tempo;
            double tempo = Tempo;

            while (UseEffect)
            {
                double spectrumDelta = (timer * tempo) / 1000;
                double r, g, b;
                (r, g, b) = GetSpectrumColorF(spectrumDelta);

                if (HasChroma)
                {
                    ChromaFX.Color coraleColor = new ChromaFX.Color(r, g, b);
                    //ChromaFX.Devices.Mouse.Mouse.Instance.SetAll(coraleColor);
                    //ChromaFX.Devices.ChromaLink.ChromaLink.Instance.SetAll(coraleColor);
                    ////ChromaFX.Devices.Mouse.Ch.SetAll(coraleColor);

                    //Custom custom = new Custom(coraleColor);
                    //speakers.SetStatic(custom, ChromaFX.Devices.Devices.Nommo);
                    //Chroma.Instance.SetAll(coraleColor);
                    //Chroma.Instance.Headset.SetAll(coraleColor);
                    //Chroma.Instance.Mouse.SetAll(coraleColor);
                    //Chroma.Instance.ChromaLink.SetAll(coraleColor);

                    //ChromaFX.Devices.Mouse.Mouse.Instance.SetAll(coraleColor);
                    //ChromaFX.Devices.ChromaLink.ChromaLink.Instance.SetAll(coraleColor);
                    //ChromaFX.Devices.Headset.Headset.Instance.SetAll(coraleColor);
                    //Custom custom = new Custom(coraleColor);
                    //speakers.SetStatic(custom, ChromaFX.Devices.Devices.Nommo);
                    Chroma.Instance.SetAll(coraleColor);
                }

                if (HasLightFX)
                {
                    AlienFXLightingControl.SetFXColour((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
                }

                if (HasHue)
                {
                    HueFX.Instance.SetColour(r, g, b);
                }

                Thread.Sleep(refreshRate);
                timer += refreshRate;

                if (timer > timerLimit)
                {
                    timer -= timerLimit;
                }
            }
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

            return Color.FromArgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        private static (double r, double g, double b) GetSpectrumColorF(double w)
        {
            double r = 0.0f;
            double g = 0.0f;
            double b = 0.0f;

            w = w % 100;

            if (w < 17)
            {
                r = -(w - 17.0) / 17.0;
                b = 1.0f;
            }
            else if (w < 33)
            {
                g = (w - 17.0) / (33.0 - 17.0);
                b = 1.0f;
            }
            else if (w < 50)
            {
                g = 1.0f;
                b = -(w - 50.0) / (50.0 - 33.0);
            }
            else if (w < 67)
            {
                r = (w - 50.0) / (67.0 - 50.0);
                g = 1.0f;
            }
            else if (w < 83)
            {
                r = 1.0f;
                g = -(w - 83.0) / (83.0f - 67.0);
            }
            else
            {
                r = 1.0f;
                b = (w - 83.0) / (100.0 - 83.0);
            }

            return (r, g, b);
        }
    }
}
