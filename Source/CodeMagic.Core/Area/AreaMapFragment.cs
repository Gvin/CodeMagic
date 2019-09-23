namespace CodeMagic.Core.Area
{
    public class AreaMapFragment
    {
        public AreaMapFragment(IAreaMapCell[][] cells, int width, int height)
        {
            Width = width;
            Height = height;
            Cells = cells;
        }

        public int Width { get; }

        public int Height { get; }

        public IAreaMapCell GetCell(int x, int y)
        {
            return Cells[y][x];
        }

        private IAreaMapCell[][] Cells { get; }
    }
}