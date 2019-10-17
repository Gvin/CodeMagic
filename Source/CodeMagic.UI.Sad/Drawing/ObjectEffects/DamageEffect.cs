using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Game.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Sad.Drawing.ObjectEffects
{
    public class DamageEffect : ObjectEffect, IDamageEffect
    {
        private const int YShift = 1;

        public DamageEffect(int value, Element element)
        {
            Value = value;
            Element = element;
        }
        public int Value { get; }

        public Element Element { get; }

        public override SymbolsImage GetEffectImage(int width, int height)
        {
            var color = ItemTextHelper.GetElementColor(Element);
            var damageText = Value.ToString();

            var xShift = 1;
            if (damageText.Length == width)
            {
                xShift = 0;
            }

            if (damageText.Length > width)
            {
                damageText = "XXX";
            }

            var damageTextImage = new SymbolsImage(width, height);
            for (int shift = 0; shift < damageText.Length; shift++)
            {
                var x = xShift + shift;
                if (x >= damageTextImage.Width)
                    break;

                damageTextImage.SetPixel(x, YShift, damageText[shift], color);
            }

            return damageTextImage;
        }
    }
}