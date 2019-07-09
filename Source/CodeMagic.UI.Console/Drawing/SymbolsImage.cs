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

        public void SetSymbolMap(char[][] symbolMap)
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

        public void SetDefaultValues(char? symbol, Color? symbolColor, Color? backgroundColor)
        {
            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    if (symbol.HasValue)
                        Pixels[y][x].Symbol = symbol.Value;
                    if (symbolColor.HasValue)
                        Pixels[y][x].Color = symbolColor.Value;
                    if (backgroundColor.HasValue)
                        Pixels[y][x].BackgroundColor = backgroundColor.Value;
                }
            }
        }

        public Pixel[][] Pixels { get; }

        public Pixel GetPixel(int x, int y)
        {
            return Pixels[y][x];
        }

        public class Pixel
        {
            public Pixel()
            {
                Symbol = ' ';
                Color = Color.White;
            }

            public char Symbol { get; set; }

            public Color Color { get; set; }

            public Color? BackgroundColor { get; set; }
        }
    }
}