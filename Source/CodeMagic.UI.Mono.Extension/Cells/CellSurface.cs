using System;
using CodeMagic.UI.Mono.Extension.Glyphs;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Cells
{
    internal class CellSurface : CellSurfaceBase
    {
        private readonly Cell[,] cells;

        public CellSurface(int width, int height)
        {
            Width = width;
            Height = height;

            cells = new Cell[width, height];
            FillEmptyCells(width, height);
        }

        public override int Width { get; }

        public override int Height { get; }

        public override void Clear()
        {
            FillEmptyCells(Width, Height);
        }

        private void FillEmptyCells(int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = new Cell();
                }
            }
        }

        public override void Clear(Rectangle area)
        {
            for (int x = area.X; x < area.Width; x++)
            {
                for (int y = area.Y; y < area.Height; y++)
                {
                    SetCell(x, y, new Cell());
                }
            }
        }

        public override Cell GetCell(int x, int y)
        {
            if (!ContainsPoint(x, y))
                throw new IndexOutOfRangeException($"Coordinates are out of range: X={x}; Y={y}; Width={Width}; Height={Height}");

            return cells[x, y];
        }

        public override void SetCell(int x, int y, Cell cell, bool useConversion = true)
        {
            if (!ContainsPoint(x, y))
                throw new IndexOutOfRangeException($"Coordinates are out of range: X={x}; Y={y}; Width={Width}; Height={Height}");
            if (cell == null)
                throw new ArgumentNullException(nameof(cell));

            var glyph = cell.Glyph;
            if (useConversion)
            {
                glyph = cell.Glyph.HasValue ? GlyphsConverterManager.ConvertGlyph(cell.Glyph.Value) : (int?) null;
            }
            cells[x, y] = new Cell(glyph, cell.ForeColor, cell.BackColor);
        }
    }
}