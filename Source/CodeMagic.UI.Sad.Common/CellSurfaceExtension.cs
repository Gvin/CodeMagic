using CodeMagic.UI.Images;
using Microsoft.Xna.Framework;
using SadConsole;

namespace CodeMagic.UI.Sad.Common
{
    public static class CellSurfaceExtension
    {
        public static void DrawVerticalLine(this CellSurface surface, int x, int y, int length,
            ColoredGlyph coloredGlyph)
        {
            for (var dY = 0; dY < length; dY++)
            {
                surface.Print(x, y + dY, coloredGlyph);
            }
        }

        public static void DrawImage(this CellSurface surface, int x, int y, SymbolsImage image, Color defaultForeColor, Color defaultBackColor)
        {
            for (int posY = 0; posY < image.Height; posY++)
            {
                for (int posX = 0; posX < image.Width; posX++)
                {
                    var pixel = image[posX, posY];
                    var backColor = pixel.BackgroundColor.HasValue ? ColorHelper.ConvertToXna(pixel.BackgroundColor.Value) : defaultBackColor;

                    var printX = x + posX;
                    var printY = y + posY;

                    if (pixel.Symbol.HasValue)
                    {
                        var foreColor = pixel.Color.HasValue ? ColorHelper.ConvertToXna(pixel.Color.Value) : defaultForeColor;
                        surface.Print(printX, printY,
                            new ColoredGlyph(Glyphs.GetGlyph(pixel.Symbol.Value), foreColor, backColor));
                    }
                    else
                    {
                        surface.Print(printX, printY, new ColoredGlyph(' ', defaultForeColor, backColor));
                    }
                }
            }
        }

        public static void PrintStyledText(this CellSurface surface, int x, int y, params ColoredString[] text)
        {
            var shiftX = 0;
            foreach (var part in text)
            {
                surface.Print(x + shiftX, y, part);
                shiftX += part.Count;
            }
        }
    }
}