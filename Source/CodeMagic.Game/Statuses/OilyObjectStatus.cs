using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Statuses
{
    public class OilyObjectStatus : IBurningRelatedStatus
    {
        private const string CustomValueOilyStatusLifeTime = "OilyStatus.LifeTime";
        private const string CustomValueOilyStatusCatchFireChanceModifier = "OilyStatus.CatchFireChanceModifier";
        private const string CustomValueOilyStatusSelfExtinguishChanceModifier = "OilyStatus.SelfExtinguishChanceModifier";

        public const string StatusType = "oily";

        private readonly int maxLifeTime;
        private int lifeTime;

        public OilyObjectStatus(ILiquidConfiguration configuration)
        {
            maxLifeTime = int.Parse(GetCustomConfigurationValue(configuration, CustomValueOilyStatusLifeTime));
            CatchFireChanceModifier = double.Parse(GetCustomConfigurationValue(configuration, CustomValueOilyStatusCatchFireChanceModifier));
            SelfExtinguishChanceModifier = double.Parse(GetCustomConfigurationValue(configuration, CustomValueOilyStatusSelfExtinguishChanceModifier));

            lifeTime = 0;
        }

        public bool Update(IDestroyableObject owner, Point position)
        {
            if (lifeTime >= maxLifeTime)
            {
                return false;
            }

            lifeTime++;
            return true;
        }

        public string Type => StatusType;

        public double CatchFireChanceModifier { get; }

        public double SelfExtinguishChanceModifier { get; }

        private string GetCustomConfigurationValue(ILiquidConfiguration configuration, string key)
        {
            var stringValue = configuration.CustomValues
                .FirstOrDefault(value => string.Equals(value.Key, key))?.Value;
            if (string.IsNullOrEmpty(stringValue))
                throw new ApplicationException($"Custom value {key} not found in the configuration for \"{OilLiquid.LiquidType}\".");

            return stringValue;
        }

    }
}