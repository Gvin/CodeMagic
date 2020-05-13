using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public static class TextHelper
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
        public static readonly Color XpColor = Color.DarkGoldenrod;

        public static string GetStatName(PlayerStats stat)
        {
            switch (stat)
            {
                case PlayerStats.Strength:
                    return "Strength";
                case PlayerStats.Agility:
                    return "Agility";
                case PlayerStats.Intelligence:
                    return "Intelligence";
                case PlayerStats.Wisdom:
                    return "Wisdom";
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
            }
        }

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

            var powerDistance = GetLightPowerDistance(item.LightPower);

            descriptionResult.Add(new StyledLine
            {
                $"Light: {powerDistance}m"
            });
        }

        private static int GetLightPowerDistance(LightLevel power)
        {
            return (int) power;
        }

        private static StyledString GetEquipableBonusTypeName(EquipableBonusType bonusType)
        {
            switch (bonusType)
            {
                case EquipableBonusType.Health:
                    return new StyledString("Health", HealthColor);
                case EquipableBonusType.Mana:
                    return new StyledString("Mana", ManaColor);
                case EquipableBonusType.ManaRegeneration:
                    return new StyledString("Mana Regeneration", ManaRegenerationColor);
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }

        public static void AddBonusesDescription(IEquipableItem item, IEquipableItem equiped, List<StyledLine> descriptionResult)
        {
            var equalItems = item.Equals(equiped);

            foreach (var bonusType in Enum.GetValues(typeof(EquipableBonusType)).Cast<EquipableBonusType>())
            {
                var equipedValue = equiped?.GetBonus(bonusType) ?? 0;
                var value = item.GetBonus(bonusType);
                if (value == 0 && equipedValue == 0)
                    continue;

                var bonusName = GetEquipableBonusTypeName(bonusType);
                var bonusDescription = new StyledLine
                {
                    bonusName,
                    " Bonus: "
                };
                if (equalItems)
                {
                    bonusDescription.Add(GetValueString(value));
                }
                else
                {
                    bonusDescription.Add(GetCompareValueString(value, equipedValue));
                }
                descriptionResult.Add(bonusDescription);
            }

            foreach (var stat in Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>())
            {
                var equipedValue = equiped?.GetStatBonus(stat) ?? 0;
                var value = item.GetStatBonus(stat);
                if (value == 0 && equipedValue == 0)
                    continue;

                var bonusName = GetStatName(stat);
                var bonusDescription = new StyledLine
                {
                    bonusName,
                    " Bonus: "
                };
                if (equalItems)
                {
                    bonusDescription.Add(GetValueString(value));
                }
                else
                {
                    bonusDescription.Add(GetCompareValueString(value, equipedValue));
                }
                descriptionResult.Add(bonusDescription);
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