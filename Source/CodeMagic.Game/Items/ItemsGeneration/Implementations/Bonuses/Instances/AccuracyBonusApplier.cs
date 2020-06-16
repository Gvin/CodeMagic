using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class AccuracyBonusApplier : IBonusApplier
    {
        public const string BonusType = "AccuracyBonus";
        private const string BonusCode = "accuracy_bonus";

        private const string KeyMin = "Min";
        private const string KeyMax = "Max";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            if (!(itemConfiguration is WeaponItemConfiguration item))
                throw new ApplicationException(
                    $"{nameof(AccuracyBonusApplier)} cannot be applied to item configuration {itemConfiguration.GetType().Name}");

            var min = int.Parse(config.Values[KeyMin]);
            var max = int.Parse(config.Values[KeyMax]);
            var bonus = RandomHelper.GetRandomValue(min, max);

            if (bonus == 0)
                return;

            item.HitChance = Math.Min(100, (int)Math.Round(item.HitChance * (1 + bonus / 100d)));

            name.AddNamePrefix(BonusCode, "Balanced");
            name.AddDescription(BonusCode, GetDescription());
        }

        private string GetDescription()
        {
            return RandomHelper.GetRandomElement(
                "This item has good weight balance.", 
                "Good wight balance makes this item more accurate."
            );
        }
    }
}