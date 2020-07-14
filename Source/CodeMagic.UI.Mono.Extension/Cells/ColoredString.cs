using System.Linq;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Cells
{
    public class ColoredString
    {
        public ColoredString(params Cell[] cells)
        {
            Cells = cells.ToArray();
        }

        public ColoredString(string text, Color? foreColor = null, Color? backColor = null)
        {
            Cells = text.Select(ch => new Cell(ch, foreColor, backColor)).ToArray();
        }

        public Cell[] Cells { get; }
    }
}