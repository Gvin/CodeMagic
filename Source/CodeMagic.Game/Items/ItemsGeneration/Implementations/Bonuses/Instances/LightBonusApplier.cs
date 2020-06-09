using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class LightBonusApplier : IBonusApplier
    {
        public const string BonusType = "LightBonus";
        private const string NamePrefix = "Shining";
        private const string KeyPower = "Power";

        private const string BonusCode = "light_bonus";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            var possiblePower = GetPossibleLightPower(config);
            var power = RandomHelper.GetRandomElement(possiblePower);

            var equipableConfig = (EquipableItemConfiguration) itemConfiguration;
            equipableConfig.LightPower = power;

            name.AddNamePrefix(BonusCode, NamePrefix);
            name.AddDescription(BonusCode, "It emits some light.");
        }

        private LightLevel[] GetPossibleLightPower(IBonusConfiguration config)
        {
            var powerStrings = config.Values[KeyPower].Replace(" ", "").Split(',');
            return powerStrings.Select(powerString => Enum.Parse(typeof(LightLevel), powerString)).Cast<LightLevel>().ToArray();
        }
    }
}