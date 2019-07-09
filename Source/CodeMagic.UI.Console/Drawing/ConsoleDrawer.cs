using System.Drawing;
using Writer = Colorful.Console;

namespace CodeMagic.UI.Console.Drawing
{
    public class ConsoleDrawer : IConsoleDrawer
    {
        public void Draw(SymbolsImage image, Color backgroundColor)
        {
            var storedPosTop = Writer.CursorTop;
            var storedPosLeft = Writer.CursorLeft;

            if (image != null)
            {
                DrawImage(image, storedPosLeft, backgroundColor);
            }

            Writer.CursorTop = storedPosTop;
            Writer.CursorLeft = storedPosLeft + SymbolsImage.Size;
        }

        private void DrawImage(SymbolsImage image, int storedPosLeft, Color backgroundColor)
        {
            for (var y = 0; y < SymbolsImage.Size; y++)
            {
                for (var x = 0; x < SymbolsImage.Size; x++)
                {
                    var pixel = image.Pixels[y][x];
                    Writer.BackgroundColor = pixel.BackgroundColor.HasValue ? pixel.BackgroundColor.Value : backgroundColor;
                    Writer.ForegroundColor = pixel.Color;
                    Writer.Write(pixel.Symbol);
                }

                Writer.CursorTop++;
                Writer.CursorLeft = storedPosLeft;
            }
        }
    }
}