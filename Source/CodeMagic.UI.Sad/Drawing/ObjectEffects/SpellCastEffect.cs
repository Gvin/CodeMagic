using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Common;
using Color = System.Drawing.Color;

namespace CodeMagic.UI.Sad.Drawing.ObjectEffects
{
    public class SpellCastEffect : ObjectEffect, ISpellCastEffect
    {
        public override SymbolsImage GetEffectImage(int width, int height)
        {
            var image = new SymbolsImage(width, height);
            image.SetPixel(1, 1, (char)Glyphs.GetGlyph('☼'), Color.Violet);
            return image;
        }
    }
}