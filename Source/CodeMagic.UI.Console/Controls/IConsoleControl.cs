using System;

namespace CodeMagic.UI.Console.Controls
{
    public interface IConsoleControl
    {
        int X { get; set; }

        int Y { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        bool Visible { get; set; }

        bool Enabled { get; set; }

        void DrawStatic();

        void DrawDynamic();

        bool ProcessKeyEvent(ConsoleKeyInfo keyInfo);
    }
}