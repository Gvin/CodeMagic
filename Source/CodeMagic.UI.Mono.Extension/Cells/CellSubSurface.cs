using System;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Cells
{
    internal class CellSubSurface : CellSurfaceBase
    {
        private readonly ICellSurface underlyingSurface;
        private readonly Rectangle location;

        public CellSubSurface(ICellSurface underlyingSurface, Rectangle location)
        {
            this.underlyingSurface = underlyingSurface;
            this.location = location;
        }

        public override int Width => location.Width;

        public override int Height => location.Height;

        public override void Clear()
        {
            underlyingSurface.Clear(location);
        }

        public override void Clear(Rectangle area)
        {
            for (int x = 0; x < area.Width; x++)
            {
                for (int y = 0; y < area.Height; y++)
                {
                    var realX = area.X + x;
                    var realY = area.Y + y;
                    SetCell(realX, realY, new Cell());
                }
            }
        }

        public override Cell GetCell(int x, int y)
        {
            if (!ContainsPoint(x, y))
                throw new IndexOutOfRangeException($"Coordinates are out of range: X={x}; Y={y}; Width={Width}; Height={Height}");

            var underlyingX = x + location.X;
            var underlyingY = y + location.Y;
            return underlyingSurface.GetCell(underlyingX, underlyingY);
        }

        public override void SetCell(int x, int y, Cell cell, bool useConversion = true)
        {
            if (!ContainsPoint(x, y))
                throw new IndexOutOfRangeException($"Coordinates are out of range: X={x}; Y={y}; Width={Width}; Height={Height}");
            if (cell == null)
                throw new ArgumentNullException(nameof(cell));

            var underlyingX = x + location.X;
            var underlyingY = y + location.Y;
            underlyingSurface.SetCell(underlyingX, underlyingY, cell, useConversion);
        }
    }
}