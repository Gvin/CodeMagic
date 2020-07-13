using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Drawing
{
    public static class XnaColorExtension
    {
        public static System.Drawing.Color ToSystem(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}