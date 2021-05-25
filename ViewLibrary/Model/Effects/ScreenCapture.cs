using AlienFXWrapper;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows;
using ChromaFX;
using ChromaFX.Devices.Speakers;
using ChromaFX.DevicesInterface.Speakers;
using ViewLibrary.Model.Hue;
using ViewLibrary.Model.Settings;
using Color = System.Drawing.Color;

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
            UseEffect = true;
            ISpeakers speakers = new Speakers();
            int refreshRate = RefreshRate;
            Rectangle bounds = new Rectangle((int)MonitorRect.X, (int)MonitorRect.Y, (int)MonitorRect.Width, (int)MonitorRect.Height);
            
            while (UseEffect)
            {
                Color color = GetScreenColour(bounds);

                if (HasLightFX)
                {
                    AlienFXLightingControl.SetFXColour(color.R, color.G, color.B);
                }

                if (HasChroma)
                {
                    ChromaFX.Color coraleColor = new ChromaFX.Color(color.R, color.G, color.B);

                    
                    //Chroma.Instance.Headset.SetAll(coraleColor);
                    //ChromaFX.Devices.Mouse.Mouse.Instance.SetAll(coraleColor);
                    //ChromaFX.Devices.ChromaLink.ChromaLink.Instance.SetAll(coraleColor);
                    Chroma.Instance.SetAll(coraleColor);

                    //Custom custom = new Custom(coraleColor);
                    //speakers.SetStatic(custom, ChromaFX.Devices.Devices.Nommo);
                }

                if (HasHue)
                {
                    HueFX.Instance.SetColour(color);
                }

                Thread.Sleep(refreshRate);
            }
        }

        private Color GetScreenColour(Rectangle bounds)
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

                return singlePixel.GetPixel(0, 0);
            }
        }
    }
}
