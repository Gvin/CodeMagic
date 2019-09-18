using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Statuses
{
    public class WetObjectStatus : IObjectStatus, IBurningRelatedStatus
    {
        public const string StatusType = "wet";

        private const string CustomValueWetStatusLifeTime = "WetStatus.LifeTime";
        private const string CustomValueWetStatusCatchFileChanceModifier = "WetStatus.CatchFireChanceModifier";
        private const string CustomValueWetStatusSelfExtinguishChanceModifier = "WetStatus.SelfExtinguishChanceModifier";

        private readonly int maxLifeTime;
        private int lifeTime;

        public WetObjectStatus(ILiquidConfiguration configuration)
        {
            maxLifeTime = int.Parse(GetCustomConfigurationValue(configuration, CustomValueWetStatusLifeTime));
            CatchFireChanceModifier = int.Parse(GetCustomConfigurationValue(configuration, CustomValueWetStatusCatchFileChanceModifier));
            SelfExtinguishChanceModifier = int.Parse(GetCustomConfigurationValue(configuration, CustomValueWetStatusSelfExtinguishChanceModifier));

            lifeTime = 0;
        }

        public bool Update(IDestroyableObject owner, AreaMapCell cell, IJournal journal)
        {
            if (lifeTime >= maxLifeTime)
            {
                return false;
            }

            lifeTime++;
            return true;
        }

        public string Type => StatusType;

        public int CatchFireChanceModifier { get; }

        public int SelfExtinguishChanceModifier { get; }

        private string GetCustomConfigurationValue(ILiquidConfiguration configuration, string key)
        {
            var stringValue = configuration.CustomValues
                .FirstOrDefault(value => string.Equals(value.Key, key))?.Value;
            if (string.IsNullOrEmpty(stringValue))
                throw new ApplicationException($"Custom value {key} not found in the configuration for \"{WaterLiquidObject.LiquidType}\".");

            return stringValue;
        }
    }
}