using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CodeMagic.UI.Images.Saving.Xml;

namespace CodeMagic.UI.Images
{
    public class SymbolsImage
    {
        private readonly Pixel[][] pixels;

        public SymbolsImage(int width, int height)
        {
            Width = width;
            Height = height;

            pixels = new Pixel[Height][];
            for (var y = 0; y < Height; y++)
            {
                pixels[y] = new Pixel[Width];
                for (var x = 0; x < Width; x++)
                {
                    pixels[y][x] = new Pixel();
                }
            }
        }

        public int Width { get; }

        public int Height { get; }

        public void SetPixel(int x, int y, char symbol, Color color, Color? backgroundColor = null)
        {
            var pixel = this[x, y];
            pixel.Symbol = symbol;
            pixel.Color = color;
            pixel.BackgroundColor = backgroundColor;
        }

        public void SetSymbolMap(char?[][] symbolMap)
        {
            PerformForEachPixel((x, y, pixel) =>
            {
                pixel.Symbol = symbolMap[y][x];
            });
        }

        public void SetColorMap(Color?[][] colorMap)
        {
            PerformForEachPixel((x, y, pixel) =>
            {
                var color = colorMap[y][x];
                if (color.HasValue)
                    pixel.Color = color.Value;
            });
        }

        public void SetDefaultColor(Color color)
        {
            PerformForEachPixel((x, y, pixel) =>
            {
                pixel.Color = color;
            });
        }

        public void SetDefaultBackColor(Color? color)
        {
            PerformForEachPixel((x, y, pixel) =>
            {
                pixel.BackgroundColor = color;
            });
        }

        public void SetDefaultValues(char symbol, Color symbolColor, Color backgroundColor)
        {
            PerformForEachPixel((x, y, pixel) =>
            {
                pixel.Symbol = symbol;
                pixel.Color = symbolColor;
                pixel.BackgroundColor = backgroundColor;
            });
        }

        private void PerformForEachPixel(Action<int, int, Pixel> action)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    action(x, y, this[x, y]);
                }
            }
        }

        public Pixel this[int x, int y] => pixels[y][x];

        public static void SaveToFile(SymbolsImage image, Stream fileStream)
        {
            var savingManager = new XmlSavingManager();
            savingManager.SaveToFile(image.Width, image.Height, image.pixels, fileStream);
        }

        public static SymbolsImage LoadFromFile(Stream fileStream)
        {
            var savingManager = new XmlSavingManager();
            return savingManager.LoadFromFile(fileStream);
        }

        public static SymbolsImage Combine(SymbolsImage bottom, SymbolsImage top)
        {
            if (bottom.Width != top.Width || bottom.Height != top.Height)
                throw new ArgumentException($"Cannot combine images {bottom.Width}x{bottom.Height} and {top.Width}x{top.Height}");

            var result = new SymbolsImage(bottom.Width, bottom.Height);

            for (var x = 0; x < bottom.Width; x++)
            for (var y = 0; y < bottom.Height; y++)
            {
                var pixel = result[x, y];
                var bottomPixel = bottom[x, y];
                var topPixel = top[x, y];

                pixel.Symbol = topPixel.Symbol ?? bottomPixel.Symbol;
                pixel.Color = topPixel.Symbol.HasValue ? topPixel.Color : bottomPixel.Color;
                pixel.BackgroundColor = topPixel.BackgroundColor ?? bottomPixel.BackgroundColor;
            }

            return result;
        }

        public static SymbolsImage Recolor(SymbolsImage image, Dictionary<Color, Color> palette)
        {
            var result = new SymbolsImage(image.Width, image.Height);

            for (var x = 0; x < image.Width; x++)
            for (var y = 0; y < image.Height; y++)
            {
                var pixel = result[x, y];
                var originalPixel = image[x, y];

                pixel.Symbol = originalPixel.Symbol;

                if (originalPixel.Color.HasValue)
                {
                    pixel.Color = palette.ContainsKey(originalPixel.Color.Value)
                        ? palette[originalPixel.Color.Value]
                        : originalPixel.Color;
                }
                else
                {
                    pixel.Color = null;
                }

                if (originalPixel.BackgroundColor.HasValue)
                {
                    pixel.BackgroundColor = palette.ContainsKey(originalPixel.BackgroundColor.Value)
                        ? palette[originalPixel.BackgroundColor.Value]
                        : originalPixel.BackgroundColor;
                }
                else
                {
                    pixel.BackgroundColor = null;
                }
            }

            return result;
        }

        public class Pixel
        {
            public char? Symbol { get; set; }

            public Color? Color { get; set; }

            public Color? BackgroundColor { get; set; }
        }
    }
}