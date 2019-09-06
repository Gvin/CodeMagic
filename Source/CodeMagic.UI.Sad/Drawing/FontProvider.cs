using System;
using SadConsole;

namespace CodeMagic.UI.Sad.Drawing
{
    public static class FontProvider
    {
        private const string Normal1FontPath = @".\Resources\Fonts\Font_X1.font";

        private static FontSize fontSize;

        public static void InitializeFont()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.FontSize))
            {
                fontSize = FontSize.X1;
                Properties.Settings.Default.FontSize = fontSize.ToString();
                Properties.Settings.Default.Save();
                return;
            }

            fontSize = GetConfiguredFontSize();
        }

        public static FontSize GetConfiguredFontSize()
        {
            var parsed = Enum.TryParse(Properties.Settings.Default.FontSize, true, out FontSize result);
            if (!parsed)
            {
                result = FontSize.X1;
                Properties.Settings.Default.FontSize = result.ToString();
                Properties.Settings.Default.Save();
            }

            return result;
        }

        public static double FontVerticalMultiplier => 1d;

        public static double FontHorizontalMultiplier => 2d;

        public static Font CurrentFont => GetFontMaster().GetFont(Font.FontSizes.One);

        private static FontMaster GetFontMaster()
        {
            return Global.LoadFont(Normal1FontPath);
        }
    }

    public enum FontSize
    {
        X1,
        X0_5,
        X0_75,
        X2
    }
}