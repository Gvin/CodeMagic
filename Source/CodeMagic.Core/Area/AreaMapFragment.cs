using CodeMagic.Core.Game;

namespace CodeMagic.Core.Area
{
    public class AreaMapFragment
    {
        public AreaMapFragment(AreaMapCell[][] cells, int width, int height, Point position = null)
        {
            Width = width;
            Height = height;
            Cells = cells;

            Position = position;
        }

        public int Width { get; }

        public int Height { get; }

        public Point Position { get; }

        public AreaMapCell GetCell(int x, int y)
        {
            return Cells[y][x];
        }

        private AreaMapCell[][] Cells { get; }
    }
}