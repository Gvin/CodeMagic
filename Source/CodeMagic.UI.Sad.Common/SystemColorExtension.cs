using System.Drawing;

namespace CodeMagic.UI.Sad.Common
{
    public static class SystemColorExtension
    {
        public static SadRogue.Primitives.Color ToSad(this Color color)
        {
            return SadRogue.Primitives.Color.FromNonPremultiplied(color.R, color.G, color.B, color.A);
        }
    }
}