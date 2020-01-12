using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.Extensions
{
    public static class ColorExtensions
    {
        public static System.Windows.Media.Color ToMediaColor(this ChromaFX.Color chromaColor)
        {
            return System.Windows.Media.Color.FromRgb(chromaColor.R, chromaColor.G, chromaColor.B);
        }

        public static ChromaFX.Color ToChromaColor(this System.Windows.Media.Color mediaColor)
        {
            return new ChromaFX.Color(mediaColor.R, mediaColor.G, mediaColor.B);
        }
    }
}
