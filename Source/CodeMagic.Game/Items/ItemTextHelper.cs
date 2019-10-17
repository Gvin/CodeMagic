using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items
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
        public static readonly Color NeutralColor = Color.White;

        public static readonly Color DescriptionTextColor = Color.Gray;

        public static readonly Color HealthColor = Color.Green;
        public static readonly Color ManaColor = Color.Blue;
        public static readonly Color ManaRegenerationColor = Color.DodgerBlue;

        public static StyledLine GetWeightLine(int weight)
        {
            const double kgWeightMultiplier = 1000d;
            return new StyledLine {$"Weight: {weight / kgWeightMultiplier:F2} kg"};
        }

        public static StyledLine GetCompareWeightLine(int weight, int equipedWeight)
        {
            const double kgWeightMultiplier = 1000d;

            Color newValueColor;
            Color currentValueColor;
            if (weight == equipedWeight)
            {
                newValueColor = NeutralColor;
                currentValueColor = NeutralColor;
            }
            else
            {
                newValueColor = weight < equipedWeight ? PositiveValueColor : NegativeValueColor;
                currentValueColor = weight > equipedWeight ? PositiveValueColor : NegativeValueColor;
            }

            return new StyledLine
            {
                "Weight: ",
                new StyledString($"{weight / kgWeightMultiplier:F2}", newValueColor),
                " kg (now ",
                new StyledString($"{equipedWeight / kgWeightMultiplier:F2}", currentValueColor),
                " kg)"
            };
        }

        public static StyledString[] GetCompareValueString(int newValue, int currentValue, string valuePostfix = "", bool formatBonus = true)
        {
            Color newValueColor;
            Color currentValueColor;
            if (newValue == currentValue)
            {
                newValueColor = NeutralColor;
                currentValueColor = NeutralColor;
            }
            else
            {
                newValueColor = newValue > currentValue ? PositiveValueColor : NegativeValueColor;
                currentValueColor = newValue < currentValue ? PositiveValueColor : NegativeValueColor;
            }

            var stringNewValue = formatBonus ? FormatBonusNumber(newValue) : newValue.ToString();
            var stringCurrentValue = formatBonus ? FormatBonusNumber(currentValue) : currentValue.ToString();

            return new[]
            {
                new StyledString($"{stringNewValue}{valuePostfix}", newValueColor),
                new StyledString(" (now "),
                new StyledString($"{stringCurrentValue}{valuePostfix}", currentValueColor),
                new StyledString(")")
            };
        }

        public static StyledString[] GetValueString(int value, string valuePostfix = "", bool formatBonus = true)
        {
            var stringValue = formatBonus ? FormatBonusNumber(value) : value.ToString();
            return new[]
            {
                new StyledString($"{stringValue}{valuePostfix}", NeutralColor)
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
            var equalItems = item.Equals(equiped);

            var equipedHealthBonus = equiped?.HealthBonus ?? 0;
            if (item.HealthBonus > 0 || equipedHealthBonus > 0)
            {
                var healthBonusDescription = new StyledLine
                {
                    new StyledString("Health", HealthColor),
                    " Bonus: "
                };
                if (equalItems)
                {
                    healthBonusDescription.Add(GetValueString(item.HealthBonus));
                }
                else
                {
                    healthBonusDescription.Add(GetCompareValueString(item.HealthBonus, equipedHealthBonus));
                }
                descriptionResult.Add(healthBonusDescription);
            }

            var equipedManaBonus = equiped?.ManaBonus ?? 0;
            if (item.ManaBonus > 0 || equipedManaBonus > 0)
            {
                var manaBonusDescription = new StyledLine
                {
                    new StyledString("Mana", ManaColor),
                    " Bonus: "
                };
                if (equalItems)
                {
                    manaBonusDescription.Add(GetValueString(item.ManaBonus));
                }
                else
                {
                    manaBonusDescription.Add(GetCompareValueString(item.ManaBonus, equipedManaBonus));
                }
                descriptionResult.Add(manaBonusDescription);
            }

            var equipedManaRegenBonus = equiped?.ManaRegenerationBonus ?? 0;
            if (item.ManaRegenerationBonus > 0 || equipedManaRegenBonus > 0)
            {
                var manaRegenBonusDescription = new StyledLine
                {
                    new StyledString("Mana Regeneration", ManaRegenerationColor),
                    " Bonus: "
                };
                if (equalItems)
                {
                    manaRegenBonusDescription.Add(GetValueString(item.ManaRegenerationBonus));
                }
                else
                {
                    manaRegenBonusDescription.Add(GetCompareValueString(item.ManaRegenerationBonus, equipedManaRegenBonus));
                }
                descriptionResult.Add(manaRegenBonusDescription);
            }
        }

        public static StyledLine[] ConvertDescription(string[] description)
        {
            return description.Select(line => new StyledLine
            {
                new StyledString(line, DescriptionTextColor)
            }).ToArray();
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