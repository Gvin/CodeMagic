using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CodeMagic.UI.Mono.Extension.Fonts
{
    internal class Font : IFont
    {
        private readonly int columns;
        private readonly int solidGlyphIndex;
        private readonly int padding;

        public Font(Texture2D texture, int glyphWidth, int glyphHeight, int columns, int padding, int solidGlyphIndex)
        {
            Texture = texture;
            GlyphWidth = glyphWidth;
            GlyphHeight = glyphHeight;

            this.columns = columns;
            this.solidGlyphIndex = solidGlyphIndex;
            this.padding = padding;
        }

        public int GlyphWidth { get; }

        public int GlyphHeight { get; }

        public Texture2D Texture { get; }

        public Rectangle GetGlyphRect(int glyph)
        {
            var row = (int) Math.Floor((double) glyph / columns);
            var col = glyph - row * columns;

            var x = padding + col * (GlyphWidth + padding);
            var y = padding + row * (GlyphHeight + padding);
            return new Rectangle(x, y, GlyphWidth, GlyphHeight);
        }

        public Rectangle GetEmptyGlyphRect()
        {
            return GetGlyphRect(solidGlyphIndex);
        }
    }
}