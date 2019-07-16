using System.Drawing;

namespace CodeMagic.UI.Console.Drawing.Writing.WriterImplementation
{
    public interface IWriterImplementation
    {
        void Write(int number, Color? foreColor = null, Color? backColor = null);

        void Write(string text, Color? foreColor = null, Color? backColor = null);

        void Write(char symbol, Color? foreColor = null, Color? backColor = null);

        void WriteAt(int x, int y, string text, Color? foreColor = null, Color? backColor = null);

        void WriteAt(int x, int y, char symbol, Color? foreColor = null, Color? backColor = null);

        void WriteLine(string text, Color? foreColor = null, Color? backColor = null);

        void Clear();

        int CursorX { get; set; }

        int CursorY { get; set; }

        int ScreenWidth { get; set; }

        int ScreenHeight { get; set; }

        Color ForeColor { get; set; }

        Color BackColor { get; set; }

        bool CursorVisible { get; set; }
    }
}