using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.Controls.ColorPicker
{
    public struct HsvColor
    {
        public double H;
        public double S;
        public double V;

        public HsvColor(double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }
    }
}
