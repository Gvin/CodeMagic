using System;
using CodeMagic.Core.Game;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Sad.Drawing
{
    public static class DamageColorHelper
    {
        private static readonly Color PhysicalDamageColor = Color.Red;
        private static readonly Color FireDamageColor = Color.Orange;
        private static readonly Color FrostDamageColor = Color.LightBlue;
        private static readonly Color AcidDamageColor = Color.Lime;
        private static readonly Color ElectricityDamageColor = Color.Yellow;

        public static Color GetDamageTextColor(Element element)
        {
            switch (element)
            {
                case Element.Physical:
                    return PhysicalDamageColor;
                case Element.Fire:
                    return FireDamageColor;
                case Element.Frost:
                    return FrostDamageColor;
                case Element.Acid:
                    return AcidDamageColor;
                case Element.Electricity:
                    return ElectricityDamageColor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element), $"Unknown damage element: {element}");
            }
        }
    }
}