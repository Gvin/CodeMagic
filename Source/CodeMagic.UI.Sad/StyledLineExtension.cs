using System.Linq;
using CodeMagic.Game;
using CodeMagic.UI.Sad.Common;
using SadConsole;
using SadRogue.Primitives;

namespace CodeMagic.UI.Sad
{
    public static class StyledLineExtension
    {
        public static ColoredString[] ToColoredString(this StyledLine line, Color backgroundColor)
        {
            return line.Select(part =>
                new ColoredString(part.String.ConvertGlyphs(), new ColoredString.ColoredGlyphEffect()
                {
                    Foreground = part.TextColor.ToSad(),
                    Background = backgroundColor
                })).ToArray();
        }
    }
}