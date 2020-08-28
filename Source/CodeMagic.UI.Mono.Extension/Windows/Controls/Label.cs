using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public class Label : Control
    {
        private string text;

        public Label(int x, int y)
            : base(new Rectangle(x, y, 1, 1))
        {
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

        public override void Draw(ICellSurface surface)
        {
            surface.Write(0, 0, Text, ForeColor, BackColor);
        }
    }
}