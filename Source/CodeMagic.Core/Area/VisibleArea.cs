namespace CodeMagic.Core.Area
{
    public class VisibleArea
    {
        public VisibleArea(AreaMapCell[][] cells)
        {
            Cells = cells;
        }

        public AreaMapCell[][] Cells { get; }
    }
}