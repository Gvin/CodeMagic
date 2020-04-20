using System.Collections.Generic;
using System.Drawing;
using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Saving
{
    public class SymbolsImageSaveable : ISaveable
    {
        private const string SaveKeyWidth = "Width";
        private const string SaveKeyHeight = "Height";
        private const string SaveKeyPixels = "Pixels";

        private readonly int width;
        private readonly int height;
        private readonly GridSaveable pixels;

        public SymbolsImageSaveable(SymbolsImage image)
        {
            width = image.Width;
            height = image.Height;

            var pixelsArray = new object[height][];
            for (int y = 0; y < height; y++)
            {
                pixelsArray[y] = new object[width];
                for (int x = 0; x < width; x++)
                {
                    pixelsArray[y][x] = new PixelSaveable(image[x, y]);
                }
            }
            pixels = new GridSaveable(pixelsArray);
        }

        public SymbolsImageSaveable(SaveData data)
        {
            width = data.GetIntValue(SaveKeyWidth);
            height = data.GetIntValue(SaveKeyHeight);
            pixels = data.GetObject<GridSaveable>(SaveKeyPixels);
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyWidth, width},
                {SaveKeyHeight, height},
                {SaveKeyPixels, pixels}
            });
        }

        public SymbolsImage GetImage()
        {
            var result = new SymbolsImage(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixel = (PixelSaveable) pixels.Rows[y][x];
                    result[x, y].Symbol = pixel.Symbol;
                    result[x, y].Color = pixel.Color;
                    result[x, y].BackgroundColor = pixel.BackColor;
                }
            }

            return result;
        }

        public class PixelSaveable : ISaveable
        {
            private const string SaveKeySymbol = "Symbol";
            private const string SaveKeyColor = "Color";
            private const string SaveKeyBackColor = "BackColor";

            private readonly int? color;
            private readonly int? backColor;

            public PixelSaveable(SaveData data)
            {
                var symbolData = data.GetStringValue(SaveKeySymbol);
                Symbol = symbolData != null ? int.Parse(symbolData) : (int?) null;

                var colorData = data.GetStringValue(SaveKeyColor);
                color = colorData != null ? int.Parse(colorData) : (int?)null;

                var backColorData = data.GetStringValue(SaveKeyBackColor);
                backColor = backColorData != null ? int.Parse(backColorData) : (int?)null;
            }

            public PixelSaveable(SymbolsImage.Pixel pixel)
            {
                Symbol = pixel.Symbol;
                color = pixel.Color?.ToArgb();
                backColor = pixel.BackgroundColor?.ToArgb();
            }

            public SaveDataBuilder GetSaveData()
            {
                return new SaveDataBuilder(GetType(), new Dictionary<string, object>
                {
                    {SaveKeySymbol, Symbol},
                    {SaveKeyColor, color},
                    {SaveKeyBackColor, backColor}
                });
            }

            public int? Symbol { get; }

            public Color? Color => color.HasValue ? System.Drawing.Color.FromArgb(color.Value) : (Color ?) null;

            public Color? BackColor => backColor.HasValue ? System.Drawing.Color.FromArgb(backColor.Value) : (Color?)null;
        }
    }
}