using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class DamageBonusApplier : IBonusApplier
    {
        public const string BonusType = "DamageBonus";
        private const string KeyMax = "Max";
        private const string KeyMin = "Min";
        private const string KeyDifference = "Difference";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            var weaponConfig = (WeaponItemConfiguration) itemConfiguration;

            var element = ItemGeneratorHelper.GetRandomDamageElement();
            var min = int.Parse(config.Values[KeyMin]);
            var max = int.Parse(config.Values[KeyMax]);
            var difference = int.Parse(config.Values[KeyDifference]);

            var maxValue = RandomHelper.GetRandomValue(min, max);
            var minValue = Math.Max(0, maxValue - difference);

            if (maxValue == 0)
                return;

            if (weaponConfig.MaxDamage.ContainsKey(element))
            {
                weaponConfig.MaxDamage[element] += maxValue;
                weaponConfig.MinDamage[element] += minValue;
            }
            else
            {
                weaponConfig.MaxDamage.Add(element, maxValue);
                weaponConfig.MinDamage.Add(element, minValue);
            }

            var bonusCode = GetBonusCode(element);
            name.AddNamePrefix(bonusCode, GetElementPrefix(element));
            name.AddDescription(bonusCode, GetBonusDescription(element));
        }

        private static string GetBonusCode(Element element)
        {
            return $"damage_bonus_{element}";
        }

        private static string GetBonusDescription(Element element)
        {
            var elementName = TextHelper.GetElementName(element);
            return $"It deals additional {elementName} damage to enemy.";
        }

        private string GetElementPrefix(Element element)
        {
            switch (element)
            {
                case Element.Blunt:
                    return "Crushing";
                case Element.Slashing:
                    return "Sharp";
                case Element.Piercing:
                    return "Thin";
                case Element.Fire:
                    return "Burning";
                case Element.Frost:
                    return "Icy";
                case Element.Acid:
                    return "Acid";
                case Element.Electricity:
                    return "Shocking";
                default:
                    throw new ArgumentException($"Unknown element: {element}");
            }
        }
    }
}