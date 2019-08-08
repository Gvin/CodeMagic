using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class ManaRegenerationBonusApplier : IBonusApplier
    {
        public const string BonusType = "ManaRegenerationBonus";
        private const string NamePostfix = "Energy";
        private const string KeyMin = "Min";
        private const string KeyMax = "Max";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            var equipableConfig = (EquipableItemConfiguration)itemConfiguration;

            var min = int.Parse(config.Values[KeyMin]);
            var max = int.Parse(config.Values[KeyMax]);
            var bonus = RandomHelper.GetRandomValue(min, max);

            equipableConfig.ManaRegenerationBonus += bonus;
            name.Postfixes.Add(NamePostfix);
        }
    }
}