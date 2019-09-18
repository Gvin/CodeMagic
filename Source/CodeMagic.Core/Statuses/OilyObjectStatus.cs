using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Statuses
{
    public class OilyObjectStatus : IObjectStatus, IBurningRelatedStatus
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
            CatchFireChanceModifier = int.Parse(GetCustomConfigurationValue(configuration, CustomValueOilyStatusCatchFireChanceModifier));
            SelfExtinguishChanceModifier = int.Parse(GetCustomConfigurationValue(configuration, CustomValueOilyStatusSelfExtinguishChanceModifier));

            lifeTime = 0;
        }

        public bool Update(IDestroyableObject owner, IAreaMapCell cell, IJournal journal)
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
                throw new ApplicationException($"Custom value {key} not found in the configuration for \"{OilLiquidObject.LiquidType}\".");

            return stringValue;
        }

    }
}