using CodeMagic.Game;
using CodeMagic.UI.Images;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Drawing
{
    public static class CellSurfaceExtension
    {
        public static void Write(this ICellSurface surface, int x, int y, StyledLine line)
        {
            surface.Write(x, y, line.ToColoredString());
        }

        public static void DrawImage(this ICellSurface surface, int x, int y, SymbolsImage image, Color defaultFore, Color defaultBack)
        {
            for (int posY = 0; posY < image.Height; posY++)
            {
                for (int posX = 0; posX < image.Width; posX++)
                {
                    var pixel = image[posX, posY];
                    var backColor = pixel.BackgroundColor?.ToXna() ?? defaultBack;

                    var printX = x + posX;
                    var printY = y + posY;

                    if (pixel.Symbol.HasValue)
                    {
                        var foreColor = pixel.Color?.ToXna() ?? defaultFore;
                        surface.SetCell(printX, printY, new Cell(pixel.Symbol.Value, foreColor, backColor), false);
                    }
                    else
                    {
                        surface.SetCell(printX, printY, new Cell(' ', defaultFore, backColor), false);
                    }
                }
            }
        }
    }
}