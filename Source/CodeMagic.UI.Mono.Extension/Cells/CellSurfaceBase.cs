using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Cells
{
    internal abstract class CellSurfaceBase : ICellSurface
    {
        public abstract int Width { get; }

        public abstract int Height { get; }

        public abstract void Clear();

        public abstract void Clear(Rectangle area);

        public abstract Cell GetCell(int x, int y);

        public abstract void SetCell(int x, int y, Cell cell, bool useConversion = true);

        public void SetCell(int x, int y, int? glyph = null, Color? foreColor = null, Color? backColor = null,
            bool useConversion = true)
        {
            SetCell(x, y, new Cell(glyph, foreColor, backColor), useConversion);
        }

        public bool ContainsPoint(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public ICellSurface GetSubSurface(Rectangle subSurfaceLocation)
        {
            return new CellSubSurface(this, subSurfaceLocation);
        }

        public void Write(int x, int y, string text, Color? foreColor = null, Color? backColor = null)
        {
            for (int index = 0; index < text.Length; index++)
            {
                SetCell(x + index, y, new Cell(text[index], foreColor, backColor));
            }
        }

        public void Write(int x, int y, params ColoredString[] strings)
        {
            int dX = 0;

            foreach (var coloredString in strings)
            {
                foreach (var cell in coloredString.Cells)
                {
                    SetCell(x + dX, y, cell);
                    dX++;
                }
            }
        }

        public void Fill(Rectangle area, Cell cell)
        {
            for (int x = 0; x < area.Width; x++)
            {
                for (int y = 0; y < area.Height; y++)
                {
                    var realX = area.X + x;
                    var realY = area.Y + y;

                    
                    SetCell(realX, realY, cell);
                }
            }
        }
    }
}