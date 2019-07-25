using System;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Sad.Common
{
    public static class ColorHelper
    {
        public static Color FromRgb(int r, int g, int b)
        {
            var color = System.Drawing.Color.FromArgb(r, g, b);
            return ConvertColor(color);
        }

        public static Color ConvertColor(System.Drawing.Color color)
        {
            return ConvertFromHex(ConvertToHex(color));
        }

        private static Color ConvertFromHex(string s)
        {
            if (s.Length != 7)
                return Color.Gray;

            int r = Convert.ToInt32(s.Substring(1, 2), 16);
            int g = Convert.ToInt32(s.Substring(3, 2), 16);
            int b = Convert.ToInt32(s.Substring(5, 2), 16);
            return new Color(r, g, b);
        }

        private static String ConvertToHex(System.Drawing.Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}