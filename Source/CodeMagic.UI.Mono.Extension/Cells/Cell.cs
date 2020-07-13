using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Cells
{
    public class Cell
    {
        public Cell(char glyph, Color? foreColor = null, Color? backColor = null)
        {
            Glyph = glyph;
            ForeColor = foreColor;
            BackColor = backColor;
        }

        public Cell(int? glyph = null, Color? foreColor = null, Color? backColor = null)
        {
            Glyph = glyph;
            ForeColor = foreColor;
            BackColor = backColor;
        }

        public int? Glyph { get; set; }

        public Color? ForeColor { get; set; }

        public Color? BackColor { get; set; }

        public Cell Clone()
        {
            return new Cell(Glyph, ForeColor, BackColor);
        }
    }
}