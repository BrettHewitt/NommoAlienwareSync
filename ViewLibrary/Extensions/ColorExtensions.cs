namespace ViewLibrary.Extensions
{
    public static class ColorExtensions
    {
        //public static Corale.Colore.Core.Color ToCoraleColor(this System.Drawing.Color drawingColor)
        //{
        //    return new Corale.Colore.Core.Color(drawingColor.R, drawingColor.G, drawingColor.B);
        //}

        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color chromaColor)
        {
            return System.Windows.Media.Color.FromRgb(chromaColor.R, chromaColor.G, chromaColor.B);
        }

        public static System.Drawing.Color ToDrawingColor(this System.Windows.Media.Color mediaColor)
        {
            return System.Drawing.Color.FromArgb(mediaColor.R, mediaColor.G, mediaColor.B);
        }
    }
}
