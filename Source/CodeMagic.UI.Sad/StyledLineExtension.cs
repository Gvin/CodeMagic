using System.Linq;
using CodeMagic.Game;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using SadConsole;

namespace CodeMagic.UI.Sad
{
    public static class StyledLineExtension
    {
        public static ColoredString[] ToColoredString(this StyledLine line, Color backgroundColor)
        {
            return line.Select(part =>
                new ColoredString(part.String.ConvertGlyphs(), part.TextColor.ToXna(), backgroundColor)).ToArray();
        }
    }
}