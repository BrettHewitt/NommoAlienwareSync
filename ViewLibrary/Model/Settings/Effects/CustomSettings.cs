using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ViewLibrary.Model.Settings.Effects
{
    public class CustomSettings
    {
        public double Tempo
        {
            get;
            set;
        } = 0.0001;

        public Color[] Colours
        {
            get;
            set;
        } = new Color[0];
    }
}
