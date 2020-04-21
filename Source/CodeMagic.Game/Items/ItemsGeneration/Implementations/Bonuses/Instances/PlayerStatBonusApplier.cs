using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class PlayerStatBonusApplier : IBonusApplier
    {
        public const string BonusType = "PlayerStatBonus";

        private const string KeyMin = "Min";
        private const string KeyMax = "Max";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            var equipableConfig = (EquipableItemConfiguration)itemConfiguration;

            var bonusType = GenerateStat();

            var min = int.Parse(config.Values[KeyMin]);
            var max = int.Parse(config.Values[KeyMax]);
            var bonus = RandomHelper.GetRandomValue(min, max);

            if (bonus == 0)
                return;

            if (!equipableConfig.StatBonuses.ContainsKey(bonusType))
            {
                equipableConfig.StatBonuses.Add(bonusType, 0);
            }
            equipableConfig.StatBonuses[bonusType] += bonus;
            name.Postfixes.Add(GetNamePostfix(bonusType));
        }

        private static PlayerStats GenerateStat()
        {
            return RandomHelper.GetRandomEnumValue<PlayerStats>();
        }

        private static string GetNamePostfix(PlayerStats statType)
        {
            return TextHelper.GetStatName(statType);
        }
    }
}