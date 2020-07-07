using System;
using System.Collections.Generic;
using CodeMagic.UI.Services;
using SadConsole;

namespace CodeMagic.UI.Sad.Drawing
{
    public static class FontProvider
    {
        private const int BaseScreenWidth = 140;
        private const int BaseScreenHeight = 76;

        private const int FontX2Size = 32;
        private const int FontX1Size = 16;
        private const int FontX075Size = 12;
        private const int FontX050Size = 8;

        private const string FontX1Path = @".\Resources\Fonts\Font_X1.font";
        private const string FontX075Path = @".\Resources\Fonts\Font_X075.font";
        private const string FontX050Path = @".\Resources\Fonts\Font_X050.font";

        private static readonly Dictionary<FontSize, Font> FontsCache = new Dictionary<FontSize, Font>();

        public static int GetScreenWidth(FontTarget fontTarget)
        {
            var widthInPx = BaseScreenWidth * GetFontSizePixels(FontTarget.Game);
            return (int) Math.Floor(widthInPx / (double) GetFontSizePixels(fontTarget));
        }

        public static int GetScreenHeight(FontTarget fontTarget)
        {
            var heightInPx = BaseScreenHeight * GetFontSizePixels(FontTarget.Game);
            return (int) Math.Floor(heightInPx / (double) GetFontSizePixels(fontTarget));
        }

        private static int GetFontSizePixels(FontTarget target)
        {
            var fontSize = GetFontSize(target);
            return GetFontPixels(fontSize);
        }

        public static Font GetFont(FontTarget target)
        {
            var fontSize = GetFontSize(target);
            return GetFontForSize(fontSize);
        }

        private static FontSize GetFontSize(FontTarget target)
        {
            switch (Settings.Current.FontSize)
            {
                case FontSizeMultiplier.X1:
                    switch (target)
                    {
                        case FontTarget.Game:
                            return FontSize.X050;
                        case FontTarget.Interface:
                            return FontSize.X075;
                        default:
                            throw new ApplicationException($"Unrecognized font size: {target}");
                    }
                case FontSizeMultiplier.X2:
                    switch (target)
                    {
                        case FontTarget.Game:
                            return FontSize.X075;
                        case FontTarget.Interface:
                            return FontSize.X1;
                        default:
                            throw new ApplicationException($"Unrecognized font size: {target}");
                    }
                default:
                    throw new ApplicationException($"Unrecognized font size multiplier: {Settings.Current.FontSize}");
            }
        }

        private static Font GetFontForSize(FontSize size)
        {
            if (!FontsCache.ContainsKey(size))
            {
                var font = LoadFont(size);
                FontsCache.Add(size, font);
            }

            return FontsCache[size];
        }

        private static Font LoadFont(FontSize size)
        {
            switch (size)
            {
                case FontSize.X050:
                    return Global.LoadFont(FontX050Path).GetFont(Font.FontSizes.One);
                case FontSize.X075:
                    return Global.LoadFont(FontX075Path).GetFont(Font.FontSizes.One);
                case FontSize.X1:
                    return Global.LoadFont(FontX1Path).GetFont(Font.FontSizes.One);
                case FontSize.X2:
                    return Global.LoadFont(FontX1Path).GetFont(Font.FontSizes.Two);
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, null);
            }
        }

        private static int GetFontPixels(FontSize size)
        {
            switch (size)
            {
                case FontSize.X050:
                    return FontX050Size;
                case FontSize.X075:
                    return FontX075Size;
                case FontSize.X1:
                    return FontX1Size;
                case FontSize.X2:
                    return FontX2Size;
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, null);
            }
        }

        private enum FontSize
        {
            X050,
            X075,
            X1,
            X2
        }
    }

    public enum FontTarget
    {
        Game,
        Interface
    }
}