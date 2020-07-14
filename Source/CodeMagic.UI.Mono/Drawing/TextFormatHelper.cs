using System.Collections.Generic;
using System.Linq;
using CodeMagic.Game;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Drawing
{
    public static class TextFormatHelper
    {
        public static ColoredString[] SplitText(StyledLine[] text, int width, Color backgroundColor)
        {
            return text.Select(line =>
                line.Select(part => 
                        new ColoredString(part.String, part.TextColor.ToXna(), backgroundColor))
                    .ToArray())
                .SelectMany(line => SplitText(line, width)).ToArray();
        }
    
        public static ColoredString[] SplitText(ColoredString[] text, int width)
        {
            var glyphs = text.SelectMany(part => part.Cells).ToArray();
    
            var result = new List<Cell[]>();
            var accumulator = new List<Cell>();
            foreach (var glyph in glyphs)
            {
                if (accumulator.Count < width)
                {
                    accumulator.Add(glyph);
                    continue;
                }
    
                var lastSpaceIndex = accumulator.FindLastIndex(elem => elem.Glyph == ' ');
                var newAccumulator = accumulator.Skip(lastSpaceIndex + 1).ToList();
    
                result.Add(accumulator.Take(lastSpaceIndex + 1).ToArray());
                accumulator = newAccumulator;
                accumulator.Add(glyph);
            }
            result.Add(accumulator.ToArray());
    
            return result.Select(line => new ColoredString(line)).ToArray();
        }
    }
}