using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Cells
{
    public interface ICellSurface
    {
        int Width { get; }

        int Height { get; }

        void Clear();

        void Clear(Rectangle area);

        Cell GetCell(int x, int y);

        void SetCell(int x, int y, Cell cell);

        bool ContainsPoint(int x, int y);

        ICellSurface GetSubSurface(Rectangle subSurfaceLocation);

        void Write(int x, int y, string text, Color? foreColor = null, Color? backColor = null);

        void Write(int x, int y, params ColoredString[] strings);

        void Fill(Rectangle area, Cell cell);
    }
}