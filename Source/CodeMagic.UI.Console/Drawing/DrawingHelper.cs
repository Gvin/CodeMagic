using System.Drawing;
using Writer = Colorful.Console;

namespace CodeMagic.UI.Console.Drawing
{
    public static class DrawingHelper
    {
        public static void DrawImageAt(int x, int y, SymbolsImage image, Color defaultBackgroundColor)
        {
            if (image == null)
                return;

            Writer.CursorTop = y;
            Writer.CursorLeft = x;

            for (var indexY = 0; indexY < SymbolsImage.Size; indexY++)
            {
                for (var indexX = 0; indexX < SymbolsImage.Size; indexX++)
                {
                    var pixel = image.Pixels[indexY][indexX];
                    Writer.BackgroundColor = pixel.BackgroundColor.HasValue ? pixel.BackgroundColor.Value : defaultBackgroundColor;
                    Writer.Write(pixel.Symbol, pixel.Color);
                }

                Writer.CursorTop++;
                Writer.CursorLeft = x;
            }
        }

        public static void DrawVerticalLine(int x, int startY, int endY, bool @double, Color? color = null,
            Color? backColor = null)
        {
            var symbol = @double ? LineTypes.DoubleVertical : LineTypes.SingleVertical;
            DrawVerticalLine(x, startY, endY, symbol, color, backColor);
        }

        public static void DrawHorizontalLine(int y, int startX, int endX, bool @double, Color? color = null,
            Color? backColor = null)
        {
            var symbol = @double ? LineTypes.DoubleHorizontal : LineTypes.SingleHorizontal;
            DrawHorizontalLine(y, startX, endX, symbol, color, backColor);
        }

        public static void DrawVerticalLine(int x, int startY, int endY, char symbol, Color? color = null, Color? backColor = null)
        {
            if (color.HasValue)
            {
                Writer.ForegroundColor = color.Value;
            }

            if (backColor.HasValue)
            {
                Writer.BackgroundColor = backColor.Value;
            }

            for (var y = startY; y <= endY; y++)
            {
                Writer.CursorLeft = x;
                Writer.CursorTop = y;
                Writer.Write(symbol);
            }
        }

        public static void DrawHorizontalLine(int y, int startX, int endX, char symbol, Color? color = null, Color? backColor = null)
        {
            if (color.HasValue)
            {
                Writer.ForegroundColor = color.Value;
            }

            if (backColor.HasValue)
            {
                Writer.BackgroundColor = backColor.Value;
            }

            for (var x = startX; x <= endX; x++)
            {
                Writer.CursorLeft = x;
                Writer.CursorTop = y;
                Writer.Write(symbol);
            }
        }

        public static void WriteAt(char symbol, int x, int y, Color? color = null, Color? backColor = null)
        {
            if (color.HasValue)
            {
                Writer.ForegroundColor = color.Value;
            }

            if (backColor.HasValue)
            {
                Writer.BackgroundColor = backColor.Value;
            }

            Writer.CursorLeft = x;
            Writer.CursorTop = y;
            Writer.Write(symbol);
        }
    }
}