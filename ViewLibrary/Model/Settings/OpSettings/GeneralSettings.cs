using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.Model.Settings.OpSettings
{
    public class GeneralSettings
    {
        public bool StartOnStartup
        {
            get;
            set;
        } = false;

        public bool StartEffectOnStartup
        {
            get;
            set;
        } = false;
    }
}
