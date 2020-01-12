using AlienFXWrapper;
using ChromaFX;
using ChromaFX.Devices.Speakers;
using ChromaFX.DevicesInterface.Speakers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ViewLibrary.Model.Settings;

namespace ViewLibrary.Model.Effects
{
    public class ScreenCapture : EffectBase
    {
        private readonly object _MonitorRectLock = new object();
        private Rect _MonitorRect;
        public Rect MonitorRect
        {
            get
            {
                lock (_MonitorRectLock)
                {
                    return _MonitorRect;
                }
            }
            set
            {
                lock (_MonitorRectLock)
                {
                    _MonitorRect = value;
                }
            }
        }

        public ScreenCapture() : base("Screen Capture")
        {

        }

        protected override string SetDescription()
        {
            return "Capture the average colour of a section of the screen and use this as the light colour";
        }

        protected override void LoadSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            MonitorRect = settings.ScreenCaptureSettings.MonitorRect;
        }

        public override void SaveSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            settings.ScreenCaptureSettings.MonitorRect = MonitorRect;
            SettingsManager.SaveSettings(settings);
        }

        public override void EffectAction()
        {
            SaveSettings();
            UseEffect = true;
            ISpeakers speaker = new Speakers();

            int refreshRate = RefreshRate;
            Rectangle bounds = new Rectangle((int)MonitorRect.X, (int)MonitorRect.Y, (int)MonitorRect.Width, (int)MonitorRect.Height);

            Initialise();

            while (UseEffect)
            {
                ChromaFX.Color color = GetScreenColour(bounds);

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
            }

            Uninitialise();
        }

        private ChromaFX.Color GetScreenColour(Rectangle bounds)
        {
            using (Bitmap singlePixel = new Bitmap(1, 1))
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
                }

                using (Graphics g = Graphics.FromImage(singlePixel))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    g.DrawImage(bitmap, new Rectangle(0, 0, 1, 1));
                }

                System.Drawing.Color pixel = singlePixel.GetPixel(0, 0);
                return new ChromaFX.Color(pixel.R, pixel.G, pixel.B);
            }
        }
    }
}
