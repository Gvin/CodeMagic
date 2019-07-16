using System.Drawing;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Controls
{
    public class StaticTextBlockControl : ConsoleControl
    {
        private static readonly Color DefaultBackColor = Color.Black;
        private static readonly Color DefaultTextColor = Color.White;

        public StaticTextBlockControl()
        {
            TextColor = new Color[0];
            BackColor = new Color[0];
            Lines = new string[0];
        }

        public string[] Lines { get; set; }

        public Color[] TextColor { get; set; }

        public Color[] BackColor { get; set; }

        protected override void DrawStatic(IControlWriter writer)
        {
            base.DrawStatic(writer);

            writer.CursorY = 0;
            writer.CursorX = 0;
            for (var index = 0; index < Lines.Length; index++)
            {
                var line = Lines[index];
                var color = index < TextColor.Length ? TextColor[index] : DefaultTextColor;
                var backColor = index < BackColor.Length ? BackColor[index] : DefaultBackColor;

                writer.WriteLine(line, color, backColor);
            }
        }
    }
}