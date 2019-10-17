using System;
using System.Drawing;
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

        private static readonly Color[] LightColors =
        {
            Color.Red,
            Color.Yellow,
            Color.Green,
            Color.Blue
        };

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            var possiblePower = GetPossibleLightPower(config);
            var power = RandomHelper.GetRandomElement(possiblePower);

            var color = RandomHelper.GetRandomElement(LightColors);

            var equipableConfig = (EquipableItemConfiguration) itemConfiguration;
            equipableConfig.LightColor = color;
            equipableConfig.LightPower = power;

            name.Prefixes.Add(NamePrefix);
        }

        private LightLevel[] GetPossibleLightPower(IBonusConfiguration config)
        {
            var powerStrings = config.Values[KeyPower].Replace(" ", "").Split(',');
            return powerStrings.Select(powerString => Enum.Parse(typeof(LightLevel), powerString)).Cast<LightLevel>().ToArray();
        }
    }
}