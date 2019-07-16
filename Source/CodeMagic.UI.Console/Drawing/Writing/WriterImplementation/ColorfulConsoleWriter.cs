using System.Drawing;
using ConsoleWriter = Colorful.Console;

namespace CodeMagic.UI.Console.Drawing.Writing.WriterImplementation
{
    public class ColorfulConsoleWriter : IWriterImplementation
    {
        public void Write(string text, Color? foreColor = null, Color? backColor = null)
        {
            if (foreColor.HasValue)
            {
                ForeColor = foreColor.Value;
            }

            if (backColor.HasValue)
            {
                BackColor = backColor.Value;
            }
            ConsoleWriter.Write(text);
        }

        public void Write(int number, Color? foreColor = null, Color? backColor = null)
        {
            Write(number.ToString(), foreColor, backColor);
        }

        public void Write(char symbol, Color? foreColor = null, Color? backColor = null)
        {
            Write(symbol.ToString(), foreColor, backColor);
        }

        public void WriteAt(int x, int y, string text, Color? foreColor = null, Color? backColor = null)
        {
            CursorX = x;
            CursorY = y;
            Write(text, foreColor, backColor);
        }

        public void WriteAt(int x, int y, char symbol, Color? foreColor = null, Color? backColor = null)
        {
            CursorX = x;
            CursorY = y;
            Write(symbol, foreColor, backColor);
        }

        public void WriteLine(string text, Color? foreColor = null, Color? backColor = null)
        {
            if (foreColor.HasValue)
            {
                ForeColor = foreColor.Value;
            }

            if (backColor.HasValue)
            {
                BackColor = backColor.Value;
            }
            ConsoleWriter.WriteLine(text);
        }

        public void Clear()
        {
            ConsoleWriter.Clear();
        }

        public int CursorX
        {
            get => ConsoleWriter.CursorLeft;
            set => ConsoleWriter.CursorLeft = value;
        }

        public int CursorY
        {
            get => ConsoleWriter.CursorTop;
            set => ConsoleWriter.CursorTop = value;
        }

        public int ScreenWidth
        {
            get => ConsoleWriter.WindowWidth;
            set
            {
                ConsoleWriter.WindowWidth = value;
                ConsoleWriter.BufferWidth = value;
            }
        }
        public int ScreenHeight
        {
            get => ConsoleWriter.WindowHeight;
            set
            {
                ConsoleWriter.WindowHeight = value;
                ConsoleWriter.BufferHeight = value;
            }
        }

        public Color ForeColor
        {
            get => ConsoleWriter.ForegroundColor;
            set => ConsoleWriter.ForegroundColor = value;
        }

        public Color BackColor
        {
            get => ConsoleWriter.BackgroundColor;
            set => ConsoleWriter.BackgroundColor = value;
        }

        public bool CursorVisible
        {
            get => ConsoleWriter.CursorVisible;
            set => ConsoleWriter.CursorVisible = value;
        }
    }
}