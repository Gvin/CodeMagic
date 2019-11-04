using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class HealthBonusApplier : IBonusApplier
    {
        public const string BonusType = "HealthBonus";
        private const string NamePostfix = "Life";
        private const string KeyMin = "Min";
        private const string KeyMax = "Max";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            var equipableConfig = (EquipableItemConfiguration) itemConfiguration;

            var min = int.Parse(config.Values[KeyMin]);
            var max = int.Parse(config.Values[KeyMax]);
            var bonus = RandomHelper.GetRandomValue(min, max);

            equipableConfig.HealthBonus += bonus;
            name.Postfixes.Add(NamePostfix);
        }
    }
}