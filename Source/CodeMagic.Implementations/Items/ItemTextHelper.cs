using System;
using System.Collections.Generic;
using System.Drawing;
using CodeMagic.Core.Area;
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
        private static readonly Color MagicDamageColor = Color.Violet;

        public static readonly Color PositiveValueColor = Color.LimeGreen;
        public static readonly Color NegativeValueColor = Color.Red;

        public static readonly Color DescriptionTextColor = Color.Gray;

        public static readonly Color HealthColor = Color.Green;
        public static readonly Color ManaColor = Color.Blue;
        public static readonly Color ManaRegenerationColor = Color.DodgerBlue;

        public static StyledString[] GetComparisonString(int newValue, int currentValue)
        {
            if (newValue == currentValue)
                return new StyledString[0];

            var isUp = newValue > currentValue;
            var textColor = isUp ? PositiveValueColor : NegativeValueColor;
            return new[]
            {
                new StyledString(" (now "),
                new StyledString(currentValue.ToString(), textColor),
                new StyledString(")"),
            };
        }

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
                case Element.Magic:
                    return MagicDamageColor;
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
                case Element.Magic:
                    return "Magic";
                default:
                    throw new ArgumentException($"Unknown element: {element}");
            }
        }

        public static void AddLightBonusDescription(EquipableItem item, List<StyledLine> descriptionResult)
        {
            if (!item.IsLightOn)
                return;

            var colorName = GetLightColorName(item.LightColor);
            var powerDistance = GetLightPowerDistance(item.LightPower);

            descriptionResult.Add(new StyledLine
            {
                $"Light: {powerDistance}m ",
                new StyledString(colorName, item.LightColor)
            });
        }

        private static string GetLightColorName(Color color)
        {
            return color.Name;
        }

        private static int GetLightPowerDistance(LightLevel power)
        {
            return (int) power;
        }

        public static void AddBonusesDescription(EquipableItem item, EquipableItem equiped, List<StyledLine> descriptionResult)
        {
            var equipedHealthBonus = equiped?.HealthBonus ?? 0;
            if (item.HealthBonus > 0 || equipedHealthBonus > 0)
            {
                var healthBonusDescription = new StyledLine
                {
                    "Health Bonus: ",
                    new StyledString(FormatBonusNumber(item.HealthBonus), HealthColor),
                    GetComparisonString(item.HealthBonus, equipedHealthBonus)
                };
                descriptionResult.Add(healthBonusDescription);
            }

            var equipedManaBonus = equiped?.ManaBonus ?? 0;
            if (item.ManaBonus > 0 || equipedManaBonus > 0)
            {
                var manaBonusDescription = new StyledLine
                {
                    "Mana Bonus: ",
                    new StyledString(FormatBonusNumber(item.ManaBonus), ManaColor),
                    GetComparisonString(item.ManaBonus, equipedManaBonus)
                };
                descriptionResult.Add(manaBonusDescription);
            }

            var equipedManaRegenBonus = equiped?.ManaRegenerationBonus ?? 0;
            if (item.ManaRegenerationBonus > 0 || equipedManaRegenBonus > 0)
            {
                var manaRegenBonusDescription = new StyledLine
                {
                    "Mana Regeneration Bonus: ",
                    new StyledString(FormatBonusNumber(item.ManaRegenerationBonus), ManaRegenerationColor),
                    GetComparisonString(item.ManaRegenerationBonus, equipedManaRegenBonus)
                };
                descriptionResult.Add(manaRegenBonusDescription);
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