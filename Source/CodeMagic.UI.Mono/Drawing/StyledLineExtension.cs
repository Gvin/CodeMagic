using System.Linq;
using CodeMagic.Game;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Drawing
{
    public static class StyledLineExtension
    {
        public static ColoredString[] ToColoredString(this StyledLine line)
        {
            return line.Select(part =>
                new ColoredString(part.String, part.TextColor.ToXna())).ToArray();
        }
    }
}