using System.Drawing;

namespace CodeMagic.UI.Console.Drawing
{
    public class SymbolsImage
    {
        public const int Size = 3;

        public SymbolsImage()
        {
            Pixels = new Pixel[Size][];
            for (var y = 0; y < Size; y++)
            {
                Pixels[y] = new Pixel[Size];
                for (var x = 0; x < Size; x++)
                {
                    Pixels[y][x] = new Pixel();
                }
            }
        }

        public void SetPixel(int x, int y, char symbol, Color color, Color? backgroundColor = null)
        {
            var pixel = GetPixel(x, y);
            pixel.Symbol = symbol;
            pixel.Color = color;
            pixel.BackgroundColor = backgroundColor;
        }

        public void SetSymbolMap(char?[][] symbolMap)
        {
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    Pixels[y][x].Symbol = symbolMap[y][x];
                }
            }
        }

        public void SetColorMap(Color?[][] colorMap)
        {
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    var color = colorMap[y][x];
                    if (color.HasValue)
                        Pixels[y][x].Color = color.Value;
                }
            }
        }

        public void SetDefaultColor(Color color)
        {
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    Pixels[y][x].Color = color;
                }
            }
        }

        public void SetDefaultBackColor(Color? color)
        {
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    Pixels[y][x].BackgroundColor = color;
                }
            }
        }

        public void SetDefaultValues(char symbol, Color symbolColor, Color backgroundColor)
        {
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    Pixels[y][x].Symbol = symbol;
                    Pixels[y][x].Color = symbolColor;
                    Pixels[y][x].BackgroundColor = backgroundColor;
                }
            }
        }

        public Pixel[][] Pixels { get; }

        public Pixel GetPixel(int x, int y)
        {
            return Pixels[y][x];
        }

        public static SymbolsImage Combine(SymbolsImage bottom, SymbolsImage top)
        {
            var result = new SymbolsImage();

            for (var x = 0; x < Size; x++)
            for (var y = 0; y < Size; y++)
            {
                var pixel = result.GetPixel(x, y);
                var bottomPixel = bottom.GetPixel(x, y);
                var topPixel = top.GetPixel(x, y);

                pixel.Symbol = topPixel.Symbol ?? bottomPixel.Symbol;
                pixel.Color = topPixel.Symbol.HasValue ? topPixel.Color : bottomPixel.Color;
                pixel.BackgroundColor = topPixel.BackgroundColor ?? bottomPixel.BackgroundColor;
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