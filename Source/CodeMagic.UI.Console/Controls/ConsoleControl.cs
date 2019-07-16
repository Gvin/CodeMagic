using System;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Controls
{
    public abstract class ConsoleControl : IConsoleControl
    {
        protected ConsoleControl()
        {
            X = 0;
            Y = 0;
            Width = 1;
            Height = 1;

            Visible = true;
            Enabled = true;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool Visible { get; set; }

        public bool Enabled { get; set; }

        public void DrawStatic()
        {
            if (!Visible)
                return;

            var writer = new ControlWriter(X, Y);
            DrawStatic(writer);
        }

        protected virtual void DrawStatic(IControlWriter writer)
        {
        }

        public void DrawDynamic()
        {
            if (!Visible)
                return;

            var writer = new ControlWriter(X, Y);
            DrawDynamic(writer);
        }

        public bool ProcessKeyEvent(ConsoleKeyInfo keyInfo)
        {
            if (!Enabled)
                return false;

            return ProcessKey(keyInfo);
        }

        protected virtual bool ProcessKey(ConsoleKeyInfo keyInfo)
        {
            return false;
        }

        protected virtual void DrawDynamic(IControlWriter writer)
        {
        }
    }
}