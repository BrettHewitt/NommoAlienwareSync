using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.Model.Settings
{
    public static class RuntimeGlobals
    {
        public static bool HasLightFXSdk
        {
            get;
            set;
        }

        public static bool HasChromaSDK
        {
            get;
            set;
        }

        public static bool HasNommo
        {
            get;
            set;
        }

        public static bool HasNommoPro
        {
            get;
            set;
        }
    }
}
