using System;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class WeightBonusApplier : IBonusApplier
    {
        public const string BonusType = "WeightBonus";
        private const string NamePrefix = "Light";
        private const string KeyWeightDecrease = "WeightDecrease";

        private const string BonusCode = "weight_bonus";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            var weightDecrease = int.Parse(config.Values[KeyWeightDecrease]);
            itemConfiguration.Weight = (int) Math.Round(itemConfiguration.Weight * (100 - weightDecrease) / 100f);

            name.AddNamePrefix(BonusCode, NamePrefix);
            name.AddDescription(BonusCode, "It weights less than similar items.");
        }
    }
}