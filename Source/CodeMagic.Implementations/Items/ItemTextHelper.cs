using System;
using System.Collections.Generic;
using System.Drawing;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;

namespace CodeMagic.Implementations.Items
{
    public static class ItemTextHelper
    {
        private static readonly Color PhysicalDamageColor = Color.Red;
        private static readonly Color FireDamageColor = Color.Orange;
        private static readonly Color FrostDamageColor = Color.LightBlue;
        private static readonly Color AcidDamageColor = Color.Lime;
        private static readonly Color ElectricityDamageColor = Color.Yellow;

        public static readonly Color PositiveValueColor = Color.Green;
        public static readonly Color NegativeValueColor = Color.Red;

        public static readonly Color DescriptionTextColor = Color.Gray;

        public static readonly Color HealthColor = Color.Green;
        public static readonly Color ManaColor = Color.Blue;
        public static readonly Color ManaRegenerationColor = Color.DodgerBlue;

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

        public static void AddBonusesDescription(EquipableItem item, List<StyledString[]> descriptionResult)
        {
            if (item.HealthBonus > 0)
            {
                descriptionResult.Add(new[]
                {
                    new StyledString("Health Bonus: "),
                    new StyledString(FormatBonusNumber(item.HealthBonus), ItemTextHelper.HealthColor)
                });
            }

            if (item.ManaBonus > 0)
            {
                descriptionResult.Add(new[]
                {
                    new StyledString("Mana Bonus: "),
                    new StyledString(FormatBonusNumber(item.ManaBonus), ItemTextHelper.ManaColor)
                });
            }

            if (item.ManaRegenerationBonus > 0)
            {
                descriptionResult.Add(new[]
                {
                    new StyledString("Mana Regeneration Bonus: "),
                    new StyledString(FormatBonusNumber(item.ManaRegenerationBonus), ItemTextHelper.ManaRegenerationColor)
                });
            }
        }

        public static string FormatBonusNumber(int number)
        {
            if (number > 0)
            {
                return $"+{number}";
            }

            return number.ToString();
        }
    }
}