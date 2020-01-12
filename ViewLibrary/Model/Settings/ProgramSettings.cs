using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Model.Settings.OpSettings;

namespace ViewLibrary.Model.Settings
{
    public class ProgramSettings
    {
        public GeneralSettings GeneralSettings
        {
            get;
            set;
        }

        public LightingSettings LightingSettings
        {
            get;
            set;
        }
    }
}
