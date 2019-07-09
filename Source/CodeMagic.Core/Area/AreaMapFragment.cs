namespace CodeMagic.Core.Area
{
    public class AreaMapFragment
    {
        public AreaMapFragment(AreaMapCell[][] cells, int width, int height)
        {
            Width = width;
            Height = height;
            Cells = cells;
        }

        public int Width { get; }

        public int Height { get; }

        public AreaMapCell GetCell(int x, int y)
        {
            return Cells[y][x];
        }

        private AreaMapCell[][] Cells { get; }
    }
}