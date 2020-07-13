using System;
using System.Collections.Generic;
using CodeMagic.UI.Mono.Extension.Fonts;
using CodeMagic.UI.Services;
using Microsoft.Xna.Framework.Graphics;

namespace CodeMagic.UI.Mono.Fonts
{
    public class FontProvider
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

        public static FontProvider Instance { get; private set; }

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            Instance = new FontProvider(graphicsDevice);
        }

        private readonly Dictionary<FontSize, IFont> fontsCache;
        private readonly GraphicsDevice graphicsDevice;

        private FontProvider(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            fontsCache = new Dictionary<FontSize, IFont>();
        }

        public static int GetScreenWidthPx()
        {
            return BaseScreenWidth * GetFontSizePixels(FontTarget.Game);
        }

        public static int GetScreenWidthSymbols(FontTarget fontTarget)
        {
            var widthInPx = GetScreenWidthPx();
            return (int)Math.Floor(widthInPx / (double)GetFontSizePixels(fontTarget));
        }

        public static int GetScreenHeightPx()
        {
            return BaseScreenHeight * GetFontSizePixels(FontTarget.Game);
        }

        public static int GetScreenHeightSymbols(FontTarget fontTarget)
        {
            var heightInPx = GetScreenHeightPx();
            return (int)Math.Floor(heightInPx / (double)GetFontSizePixels(fontTarget));
        }

        private static int GetFontSizePixels(FontTarget target)
        {
            var fontSize = GetFontSize(target);
            return GetFontPixels(fontSize);
        }

        public IFont GetFont(FontTarget target)
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

        private IFont GetFontForSize(FontSize size)
        {
            if (!fontsCache.ContainsKey(size))
            {
                var font = LoadFont(size);
                fontsCache.Add(size, font);
            }

            return fontsCache[size];
        }

        private IFont LoadFont(FontSize size)
        {
            var fontService = new FontService();
            switch (size)
            {
                case FontSize.X050:
                    return fontService.LoadFont(graphicsDevice, FontX050Path);
                case FontSize.X075:
                    return fontService.LoadFont(graphicsDevice, FontX075Path);
                case FontSize.X1:
                    return fontService.LoadFont(graphicsDevice, FontX1Path);
                case FontSize.X2:
                    throw new NotImplementedException($"Font size X2 is not implemented");
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