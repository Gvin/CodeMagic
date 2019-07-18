using System.Drawing;

namespace CodeMagic.UI.Console.Drawing.Writing
{
    public interface IControlWriter
    {
        void Write(int number, Color? foreColor = null, Color? backColor = null);

        void Write(string text, Color? foreColor = null, Color? backColor = null);

        void Write(char symbol, Color? foreColor = null, Color? backColor = null);

        void WriteAt(int x, int y, string text, Color? foreColor = null, Color? backColor = null);

        void WriteAt(int x, int y, char symbol, Color? foreColor = null, Color? backColor = null);

        void WriteLine(string text, Color? foreColor = null, Color? backColor = null);

        void DrawFrame(int x, int y, int width, int height, bool @double, Color color, Color backColor);

        void ClearArea(int x, int y, int width, int height, Color backColor);

        void DrawVerticalLine(int x, int startY, int endY, bool @double, Color? color = null,
            Color? backColor = null);

        void DrawHorizontalLine(int y, int startX, int endX, bool @double, Color? color = null,
            Color? backColor = null);

        void DrawImageAt(int x, int y, SymbolsImage image, Color defaultForeColor, Color defaultBackgroundColor);

        int CursorX { get; set; }

        int CursorY { get; set; }

        Color ForeColor { get; set; }

        Color BackColor { get; set; }

        bool CursorVisible { get; set; }
    }
}