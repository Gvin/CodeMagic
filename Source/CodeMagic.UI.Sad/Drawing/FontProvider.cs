using System;
using SadConsole;

namespace CodeMagic.UI.Sad.Drawing
{
    public static class FontProvider
    {
        private const int FontX2Size = 32;
        private const int FontX1Size = 16;
        private const int FontX075Size = 12;
        private const int FontX05Size = 8;

        private const double DefaultFontWidth = 8;
        private const double DefaultFontHeight = 16;

        private const string FontX1Path = @".\Resources\Fonts\Font_X1.font";
        private const string FontX075Path = @".\Resources\Fonts\Font_X075.font";

        private static IScreenSizeProvider storedScreenSizeProvider;

        public static void InitializeFont(IScreenSizeProvider screenSizeProvider, int gameWidth, int gameHeight)
        {
            storedScreenSizeProvider = screenSizeProvider;

            if (string.IsNullOrEmpty(Properties.Settings.Default.FontSize))
            {
                var fontSize = GetBestFontSizeForScreen(gameWidth, gameHeight);
                Properties.Settings.Default.FontSize = fontSize.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private static FontSize GetBestFontSizeForScreen(int gameWidth, int gameHeight)
        {
            if (CheckIfWindowFits(gameWidth, gameHeight, FontX1Size))
                return FontSize.X1;

            if (CheckIfWindowFits(gameWidth, gameHeight, FontX075Size))
                return FontSize.X075;

            return FontSize.X05;
        }

        private static int GetFontSizePixels(FontSize size)
        {
            switch (size)
            {
                case FontSize.X05:
                    return FontX05Size;
                case FontSize.X075:
                    return FontX075Size;
                case FontSize.X1:
                    return FontX1Size;
                case FontSize.X2:
                    return FontX2Size;
                default:
                    throw new ArgumentException($"Unknown font size: {size}");
            }
        }

        private static bool CheckIfWindowFits(
            int gameWidth, int gameHeight,
            int fontSizeValue)
        {
            var gameWindowWidth = gameWidth * fontSizeValue;
            var gameWindowHeight = gameHeight * fontSizeValue;

            return storedScreenSizeProvider.FitsScreen(gameWindowWidth, gameWindowHeight);
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

        public static double FontVerticalMultiplier => GetFontSizePixels(GetConfiguredFontSize()) / DefaultFontHeight + 0.01;

        public static double FontHorizontalMultiplier => GetFontSizePixels(GetConfiguredFontSize()) / DefaultFontWidth;

        public static Font CurrentFont => GetConfiguredFont();

        private static Font GetConfiguredFont()
        {
            var fontSize = GetConfiguredFontSize();
            switch (fontSize)
            {
                case FontSize.X05:
                    return Global.LoadFont(FontX1Path).GetFont(Font.FontSizes.Half);
                case FontSize.X075:
                    return Global.LoadFont(FontX075Path).GetFont(Font.FontSizes.One);
                case FontSize.X1:
                    return Global.LoadFont(FontX1Path).GetFont(Font.FontSizes.One);
                case FontSize.X2:
                    return Global.LoadFont(FontX1Path).GetFont(Font.FontSizes.Two);
                default:
                    throw new ApplicationException($"Unrecognized font size: {fontSize}");
            }
        }
    }

    public enum FontSize
    {
        X05,
        X075,
        X1,
        X2
    }
}