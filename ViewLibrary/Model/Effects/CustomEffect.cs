﻿using AlienFXWrapper;
using System;
using System.Linq;
using System.Threading;
using ChromaFX;
using ViewLibrary.Model.Hue;
using ViewLibrary.Model.Settings;
using Color = System.Drawing.Color;

namespace ViewLibrary.Model.Effects
{
    public class CustomEffect : TempoBaseEffect
    {
        private readonly object _ColourListLock = new object();
        private Color[] _Colours;
        public Color[] Colours
        {
            get
            {
                lock (_ColourListLock)
                {
                    return _Colours;
                }
            }
            set
            {
                lock (_ColourListLock)
                {
                    _Colours = value;
                }
            }
        }

        public CustomEffect() : base("Custom")
        {

        }

        public override void EffectAction()
        {
            if (Colours == null || !Colours.Any())
            {
                throw new InvalidOperationException("Can not start a custom effect with no colours");
            }

            UseEffect = true;

            int refreshRate = RefreshRate;
            int colourCount = Colours.Length;
            double timer = 0;
            double tempo = Tempo;
            double timerLimit = 1 / Tempo;
            int topIndex = 1;
            int bottomIndex = 0;
            Color[] colours = Colours.Select(x => x).ToArray();

            while (UseEffect)
            {
                if (colourCount == 1)
                {
                    Color color = Colours[0];
                    if (HasLightFX)
                    {
                        AlienFXLightingControl.SetFXColour(color.R, color.G, color.B);
                    }

                    if (HasChroma)
                    {
                        ChromaFX.Color coraleColor = new ChromaFX.Color(color.R, color.G, color.B);
                        Chroma.Instance.SetAll(coraleColor);
                    }

                    if (HasHue)
                    {
                        HueFX.Instance.SetColour(color);
                    }

                    Thread.Sleep(1000);
                }
                else
                {
                    double lerpValue = timer * tempo;
                    if (lerpValue >= 1)
                    {
                        topIndex++;
                        bottomIndex++;

                        if (topIndex == colours.Length)
                        {
                            topIndex = 0;
                        }

                        if (bottomIndex == colours.Length)
                        {
                            bottomIndex = 0;
                        }

                        lerpValue -= 1;
                        timer -= timerLimit;
                    }
                    
                    Color color = LerpRGB(colours[bottomIndex], colours[topIndex], lerpValue);

                    if (HasLightFX)
                    {
                        AlienFXLightingControl.SetFXColour(color.R, color.G, color.B);
                    }

                    if (HasChroma)
                    {
                        ChromaFX.Color coraleColor = new ChromaFX.Color(color.R, color.G, color.B);
                        Chroma.Instance.SetAll(coraleColor);
                    }

                    if (HasHue)
                    {
                        HueFX.Instance.SetColour(color);
                    }

                    timer += refreshRate;
                }

                Thread.Sleep(refreshRate);
            }
        }

        protected override void LoadSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            Tempo = settings.CustomSettings.Tempo;
            Colours = settings.CustomSettings.Colours.Select(x => Color.FromArgb(x.R, x.G, x.B)).ToArray();
        }

        public override void SaveSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            settings.CustomSettings.Tempo = Tempo;
            settings.CustomSettings.Colours = Colours.Select(x => System.Windows.Media.Color.FromRgb(x.R, x.G, x.B)).ToArray();
            SettingsManager.SaveSettings(settings);
        }

        protected override string SetDescription()
        {
            return "Choose to cycle between any desired colour";
        }

        public static Color LerpRGB(Color a, Color b, double t)
        {
            if (t > 1)
            {
                t = 1;
            }
            else if (t < 0)
            {
                t = 0;
            }

            return Color.FromArgb
            (
            (byte)(a.R + (b.R - a.R) * t),
            (byte)(a.G + (b.G - a.G) * t),
            (byte)(a.B + (b.B - a.B) * t)
            );
        }
    }
}
