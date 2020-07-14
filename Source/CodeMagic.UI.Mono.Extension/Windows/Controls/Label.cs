using System;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public class Label : IControl
    {
        private string text;

        public Label(int x, int y)
        {
            Location = new Rectangle(x, y, 1, 1);
            Enabled = false;
            Visible = true;
        }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                Location = new Rectangle(Location.X, Location.Y, text.Length, 1);
            }
        }

        public Color? ForeColor { get; set; }

        public Color? BackColor { get; set; }

        public Rectangle Location { get; set; }

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        public void Draw(ICellSurface surface)
        {
            surface.Write(0, 0, Text, ForeColor, BackColor);
        }

        public void Update(TimeSpan elapsedTime)
        {
            // Do nothing
        }

        public bool ProcessMouse(IMouseState mouseState)
        {
            return false;
        }
    }
}