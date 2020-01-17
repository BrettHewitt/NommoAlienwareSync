using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Model.Settings.Effects;

namespace ViewLibrary.Model.Settings
{
    public class GlobalSettings
    {
        //[JsonIgnore]
        //public DeviceSettings DeviceSettings
        //{ 
        //    get;
        //    set;
        //}

        public SpectrumSettings SpectrumSettings
        {
            get;
            set;
        }

        public ScreenCaptureSettings ScreenCaptureSettings
        {
            get;
            set;
        }

        public CustomSettings CustomSettings
        {
            get;
            set;
        }

        public ProgramSettings ProgramSettings
        {
            get;
            set;
        }

        public string SelectedEffect
        {
            get;
            set;
        }
    }
}
