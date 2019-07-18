using System.Drawing;
using CodeMagic.Core.Spells;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation
{
    public class CodeSpellDrawingProcessor : IDrawingProcessor
    {
        public SymbolsImage GetImage(object @object)
        {
            return GetImage((CodeSpell) @object);
        }

        private SymbolsImage GetImage(CodeSpell spell)
        {
            var image = new SymbolsImage();

            var color = GetSpellColor(spell);

            image.SetDefaultColor(color);
            image.SetSymbolMap(new []
            {
                new char?[]{'\\', '|', '/'},
                new char?[]{'-', '*', '-'},
                new char?[]{'/', '|', '\\'}
            });

            image.SetPixel(1, 1, '\u263C', color);

            return image;
        }

        private Color GetSpellColor(CodeSpell spell)
        {
            if (spell.Mana < 5)
                return Color.BlueViolet;
            if (spell.Mana < 10)
                return Color.MediumVioletRed;
            if (spell.Mana < 20)
                return Color.Violet;
            if (spell.Mana < 40)
                return Color.HotPink;

            return Color.DeepPink;
        }
    }
}