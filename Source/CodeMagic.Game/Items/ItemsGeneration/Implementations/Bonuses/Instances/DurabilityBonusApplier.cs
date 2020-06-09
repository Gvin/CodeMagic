using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class DurabilityBonusApplier : IBonusApplier
    {
        public const string BonusType = "DurabilityBonus";
        private const string BonusCode = "durability_bonus";

        private const string KeyMin = "Min";
        private const string KeyMax = "Max";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            if (!(itemConfiguration is DurableItemConfiguration item))
                throw new ApplicationException(
                    $"{nameof(DurabilityBonusApplier)} cannot be applied to item configuration {itemConfiguration.GetType().Name}");

            var min = int.Parse(config.Values[KeyMin]);
            var max = int.Parse(config.Values[KeyMax]);
            var bonus = RandomHelper.GetRandomValue(min, max);

            if (bonus == 0)
                return;

            item.MaxDurability = (int) Math.Round(item.MaxDurability * (1 + bonus / 100d));

            name.AddNamePrefix(BonusCode, "Durable");
            name.AddDescription(BonusCode, "This item looks more durable.");
        }
    }
}