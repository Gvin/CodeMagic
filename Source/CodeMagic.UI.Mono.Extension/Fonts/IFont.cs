using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CodeMagic.UI.Mono.Extension.Fonts
{
    public interface IFont
    {
        int GlyphWidth { get; }

        int GlyphHeight { get; }

        Texture2D Texture { get; }

        Rectangle GetGlyphRect(int glyph);

        Rectangle GetEmptyGlyphRect();
    }
}