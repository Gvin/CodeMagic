using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.ObjectEffects
{
    public class DamageEffect : ObjectEffect
    {
        private readonly int value;
        private readonly Element element;

        public DamageEffect(int value, Element element)
        {
            this.value = value;
            this.element = element;
        }

        public override SymbolsImage GetEffectImage(int width, int height, IImagesStorage imagesStorage)
        {
            var color = TextHelper.GetElementColor(element);
            var damageText = value.ToString();

            var xShift = (int) Math.Floor((width - damageText.Length) / 2d);

            if (damageText.Length > width)
            {
                damageText = "XXX";
            }

            var yShift = (int) Math.Floor(height / 2d);
            var damageTextImage = new SymbolsImage(width, height);
            for (int shift = 0; shift < damageText.Length; shift++)
            {
                var x = xShift + shift;
                if (x >= damageTextImage.Width)
                    break;

                damageTextImage.SetPixel(x, yShift, damageText[shift], color);
            }

            return damageTextImage;
        }
    }
}