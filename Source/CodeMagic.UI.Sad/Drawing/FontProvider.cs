using System;
using SadConsole;

namespace CodeMagic.UI.Sad.Drawing
{
    public static class FontProvider
    {
        private static Font.FontSizes fontSize;

        public static void InitializeFont()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.FontSize))
            {
                fontSize = Font.FontSizes.One;
                Properties.Settings.Default.FontSize = fontSize.ToString();
                Properties.Settings.Default.Save();
                return;
            }

            fontSize = (Font.FontSizes) Enum.Parse(typeof(Font.FontSizes), Properties.Settings.Default.FontSize);
        }

        public static double FontSizeMultiplier
        {
            get
            {
                switch (fontSize)
                {
                    case Font.FontSizes.Quarter:
                        return 0.25d;
                    case Font.FontSizes.Half:
                        return 0.5d;
                    case Font.FontSizes.One:
                        return 1d;
                    case Font.FontSizes.Two:
                        return 2d;
                    case Font.FontSizes.Three:
                        return 3d;
                    case Font.FontSizes.Four:
                        return 4d;
                    default:
                        throw new ArgumentException($"Unknown font size: {fontSize}");
                }
            }
        }

        public static Font CurrentFont => GetFontMaster().GetFont(fontSize);

        private static FontMaster GetFontMaster()
        {
            return Global.FontDefault.Master;
        }
    }
}