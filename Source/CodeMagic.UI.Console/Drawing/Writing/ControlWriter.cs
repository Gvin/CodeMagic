using System.Drawing;

namespace CodeMagic.UI.Console.Drawing.Writing
{
    public class ControlWriter : IControlWriter
    {
        private readonly int shiftX;
        private readonly int shiftY;

        public ControlWriter(int shiftX, int shiftY)
        {
            this.shiftX = shiftX;
            this.shiftY = shiftY;

            CursorX = 0;
            CursorY = 0;
        }

        public void Write(int number, Color? foreColor = null, Color? backColor = null)
        {
            Writer.Write(number, foreColor, backColor);
        }

        public void Write(string text, Color? foreColor = null, Color? backColor = null)
        {
            Writer.Write(text, foreColor, backColor);
        }

        public void Write(char symbol, Color? foreColor = null, Color? backColor = null)
        {
            Writer.Write(symbol, foreColor, backColor);
        }

        public void WriteAt(int x, int y, string text, Color? foreColor = null, Color? backColor = null)
        {
            Writer.WriteAt(x + shiftX, y + shiftY, text, foreColor, backColor);
        }

        public void WriteAt(int x, int y, char symbol, Color? foreColor = null, Color? backColor = null)
        {
            Writer.WriteAt(x + shiftX, y + shiftY, symbol, foreColor, backColor);
        }

        public void WriteLine(string text, Color? foreColor = null, Color? backColor = null)
        {
            Writer.WriteLine(text, foreColor, backColor);
            CursorX = 0;
        }

        public void DrawFrame(int x, int y, int width, int height, bool @double, Color color, Color backColor)
        {
            Writer.DrawFrame(x + shiftX, y + shiftY, width, height, @double, color, backColor);
        }

        public void ClearArea(int x, int y, int width, int height, Color backColor)
        {
            Writer.ClearArea(x + shiftX, y + shiftY, width, height, backColor);
        }

        public void DrawVerticalLine(int x, int startY, int endY, bool @double, Color? color = null, Color? backColor = null)
        {
            Writer.DrawVerticalLine(x + shiftX, startY + shiftY, endY + shiftY, @double, color, backColor);
        }

        public void DrawHorizontalLine(int y, int startX, int endX, bool @double, Color? color = null, Color? backColor = null)
        {
            Writer.DrawHorizontalLine(y + shiftY, startX + shiftX, endX + shiftX, @double, color, backColor);
        }

        public void DrawImageAt(int x, int y, SymbolsImage image, Color defaultBackgroundColor)
        {
            Writer.DrawImageAt(x + shiftX, y + shiftY, image, defaultBackgroundColor);
        }

        public int CursorX
        {
            get => Writer.CursorX - shiftX;
            set => Writer.CursorX = value + shiftX;
        }

        public int CursorY
        {
            get => Writer.CursorY - shiftY;
            set => Writer.CursorY = value + shiftY;
        }

        public Color ForeColor
        {
            get => Writer.ForeColor;
            set => Writer.ForeColor = value;
        }

        public Color BackColor
        {
            get => Writer.BackColor;
            set => Writer.BackColor = value;
        }

        public bool CursorVisible
        {
            get => Writer.CursorVisible;
            set => Writer.CursorVisible = value;
        }
    }
}