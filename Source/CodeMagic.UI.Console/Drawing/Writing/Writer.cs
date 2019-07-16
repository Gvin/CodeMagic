using System.Drawing;
using CodeMagic.UI.Console.Drawing.Writing.WriterImplementation;

namespace CodeMagic.UI.Console.Drawing.Writing
{
    public static class Writer
    {
        private static IWriterImplementation implementation;

        public static void Initialize(IWriterImplementation writer)
        {
            implementation = writer;
        }

        public static void Write(int number, Color? foreColor = null, Color? backColor = null)
        {
            implementation.Write(number, foreColor, backColor);
        }

        public static void Write(string text, Color? foreColor = null, Color? backColor = null)
        {
            implementation.Write(text, foreColor, backColor);
        }

        public static void Write(char symbol, Color? foreColor = null, Color? backColor = null)
        {
            implementation.Write(symbol, foreColor, backColor);
        }

        public static void WriteAt(int x, int y, string text, Color? foreColor = null, Color? backColor = null)
        {
            implementation.WriteAt(x, y, text, foreColor, backColor);
        }

        public static void WriteAt(int x, int y, char symbol, Color? foreColor = null, Color? backColor = null)
        {
            implementation.WriteAt(x, y, symbol, foreColor, backColor);
        }

        public static void WriteLine(string text, Color? foreColor = null, Color? backColor = null)
        {
            implementation.WriteLine(text, foreColor, backColor);
        }

        public static void DrawImageAt(int x, int y, SymbolsImage image, Color defaultBackgroundColor)
        {
            if (image == null)
                return;

            CursorY = y;
            CursorX = x;

            for (var indexY = 0; indexY < SymbolsImage.Size; indexY++)
            {
                for (var indexX = 0; indexX < SymbolsImage.Size; indexX++)
                {
                    var pixel = image.Pixels[indexY][indexX];
                    BackColor = pixel.BackgroundColor.HasValue ? pixel.BackgroundColor.Value : defaultBackgroundColor;
                    Write(pixel.Symbol, pixel.Color);
                }

                CursorY++;
                CursorX = x;
            }
        }

        public static void DrawFrame(int x, int y, int width, int height, bool @double, Color color, Color backColor)
        {
            var verticalSymbol = @double ? LineTypes.DoubleVertical : LineTypes.SingleVertical;
            var horizontalSymbol = @double ? LineTypes.DoubleHorizontal : LineTypes.SingleHorizontal;
            var topLeftSymbol = @double ? LineTypes.DoubleDownRight : LineTypes.SingleDownRight;
            var topRightSymbol = @double ? LineTypes.DoubleDownLeft : LineTypes.SingleDownLeft;
            var bottomLeftSymbol = @double ? LineTypes.DoubleUpRight : LineTypes.SingleUpRight;
            var bottomRightSymbol = @double ? LineTypes.DoubleUpLeft : LineTypes.SingleUpLeft;

            WriteAt(x, y, topLeftSymbol, color, backColor);
            WriteAt(x + width - 1, y, topRightSymbol, color, backColor);
            WriteAt(x, y + height - 1, bottomLeftSymbol, color, backColor);
            WriteAt(x + width - 1, y + height - 1, bottomRightSymbol, color, backColor);

            DrawHorizontalLine(y, x + 1, x + width - 2, horizontalSymbol, color, backColor);
            DrawHorizontalLine(y + height - 1, x + 1, x + width - 2, horizontalSymbol, color, backColor);

            DrawVerticalLine(x, y + 1, y + height - 2, verticalSymbol, color, backColor);
            DrawVerticalLine(x + width - 1, y + 1, y + height - 2, verticalSymbol, color, backColor);
        }

        public static void ClearArea(int x, int y, int width, int height, Color backColor)
        {
            var clearLine = string.Empty;
            for (var count = 0; count < width; count++)
            {
                clearLine += " ";
            }

            CursorY = y;
            for (var posY = 0; posY < height; posY++)
            {
                CursorX = x;
                WriteLine(clearLine, null, backColor);
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
                ForeColor = color.Value;
            }

            if (backColor.HasValue)
            {
                BackColor = backColor.Value;
            }

            for (var y = startY; y <= endY; y++)
            {
                CursorX = x;
                CursorY = y;
                Write(symbol);
            }
        }

        public static void DrawHorizontalLine(int y, int startX, int endX, char symbol, Color? color = null, Color? backColor = null)
        {
            if (color.HasValue)
            {
                ForeColor = color.Value;
            }

            if (backColor.HasValue)
            {
                BackColor = backColor.Value;
            }

            for (var x = startX; x <= endX; x++)
            {
                CursorX = x;
                CursorY = y;
                Write(symbol);
            }
        }

        public static void Clear()
        {
            implementation.Clear();
        }

        public static int CursorX
        {
            get => implementation.CursorX;
            set => implementation.CursorX = value;
        }

        public static int CursorY
        {
            get => implementation.CursorY;
            set => implementation.CursorY = value;
        }

        public static int ScreenWidth
        {
            get => implementation.ScreenWidth;
            set => implementation.ScreenWidth = value;
        }

        public static int ScreenHeight
        {
            get => implementation.ScreenHeight;
            set => implementation.ScreenHeight = value;
        }

        public static Color ForeColor
        {
            get => implementation.ForeColor;
            set => implementation.ForeColor = value;
        }

        public static Color BackColor
        {
            get => implementation.BackColor;
            set => implementation.BackColor = value;
        }

        public static bool CursorVisible
        {
            get => implementation.CursorVisible;
            set => implementation.CursorVisible = value;
        }
    }
}