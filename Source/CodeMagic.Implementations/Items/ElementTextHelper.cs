using System;
using System.Drawing;
using CodeMagic.Core.Game;

namespace CodeMagic.Implementations.Items
{
    public static class ElementTextHelper
    {
        private static readonly Color PhysicalDamageColor = Color.Red;
        private static readonly Color FireDamageColor = Color.Orange;
        private static readonly Color FrostDamageColor = Color.LightBlue;
        private static readonly Color AcidDamageColor = Color.Lime;
        private static readonly Color ElectricityDamageColor = Color.Yellow;

        public static readonly Color PositiveValueColor = Color.Green;
        public static readonly Color NegativeValueColor = Color.Red;

        public static readonly Color DescriptionTextColor = Color.Gray;

        public static Color GetElementColor(Element element)
        {
            switch (element)
            {
                case Element.Blunt:
                case Element.Slashing:
                case Element.Piercing:
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

        public static string GetElementName(Element element)
        {
            switch (element)
            {
                case Element.Blunt:
                    return "Blunt";
                case Element.Slashing:
                    return "Slashing";
                case Element.Piercing:
                    return "Piercing";
                case Element.Fire:
                    return "Fire";
                case Element.Frost:
                    return "Frost";
                case Element.Acid:
                    return "Acid";
                case Element.Electricity:
                    return "Electricity";
                default:
                    throw new ArgumentException($"Unknown element: {element}");
            }
        }
    }
}