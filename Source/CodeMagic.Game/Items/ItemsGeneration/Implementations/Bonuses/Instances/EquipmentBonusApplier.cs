using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class EquipmentBonusApplier : IBonusApplier
    {
        public const string BonusType = "EquipmentBonus";

        private const string KeyBonusType = "BonusType";
        private const string KeyMin = "Min";
        private const string KeyMax = "Max";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            var equipableConfig = (EquipableItemConfiguration) itemConfiguration;

            var bonusType = (EquipableBonusType) Enum.Parse(typeof(EquipableBonusType), config.Values[KeyBonusType]);

            var min = int.Parse(config.Values[KeyMin]);
            var max = int.Parse(config.Values[KeyMax]);
            var bonus = RandomHelper.GetRandomValue(min, max);

            if (bonus == 0)
                return;

            if (!equipableConfig.Bonuses.ContainsKey(bonusType))
            {
                equipableConfig.Bonuses.Add(bonusType, 0);
            }
            equipableConfig.Bonuses[bonusType] += bonus;

            name.Postfixes.Add(GetNamePostfix(bonusType));
            name.AddDescription(GetBonusCode(bonusType), GetBonusText(bonusType));
        }

        private static string GetBonusCode(EquipableBonusType bonusType)
        {
            return $"equipment_bonus_{bonusType}";
        }

        private static string GetBonusText(EquipableBonusType bonusType)
        {
            switch (bonusType)
            {
                case EquipableBonusType.Health:
                    return "It affects your body making it stronger.";
                case EquipableBonusType.Mana:
                    return "It allows you to store more mana.";
                case EquipableBonusType.ManaRegeneration:
                    return "It makes your body replenish mana faster.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }

        private static string GetNamePostfix(EquipableBonusType bonusType)
        {
            switch (bonusType)
            {
                case EquipableBonusType.Health:
                    return "Life";
                case EquipableBonusType.Mana:
                    return "Mana";
                case EquipableBonusType.ManaRegeneration:
                    return "Energy";
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }
    }
}