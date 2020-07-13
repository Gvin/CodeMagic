using System.Drawing;

namespace CodeMagic.UI.Mono.Drawing
{
    public static class SystemColorExtension
    {
        public static Microsoft.Xna.Framework.Color ToXna(this Color color)
        {
            return Microsoft.Xna.Framework.Color.FromNonPremultiplied(color.R, color.G, color.B, color.A);
        }
    }
}