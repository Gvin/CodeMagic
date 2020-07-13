using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Cells
{
    public class ColoredString
    {
        public ColoredString()
        {
        }

        public ColoredString(string text)
        {
            Text = text;
        }

        public ColoredString(string text, Color? foreColor)
        {
            Text = text;
            ForeColor = foreColor;
        }

        public ColoredString(string text, Color? foreColor, Color? backColor)
        {
            Text = text;
            ForeColor = foreColor;
            BackColor = backColor;
        }

        public string Text { get; set; }

        public Color? ForeColor { get; set; }

        public Color? BackColor { get; set; }
    }
}