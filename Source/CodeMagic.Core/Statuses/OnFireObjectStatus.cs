using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Area.EnvironmentData;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    public class OnFireObjectStatus : IObjectStatus
    {
        private const int CellTemperatureIncreaseMax = 100;

        public const string StatusType = "on_fire";

        private readonly int burningTemperature;
        private readonly int burnBeforeExtinguishCheck;

        private int burnTime;

        public OnFireObjectStatus(OnFireObjectStatusConfiguration configuration)
        {
            burningTemperature = configuration.BurningTemperature;
            burnBeforeExtinguishCheck = configuration.BurnBeforeExtinguishCheck;

            burnTime = 0;
        }

        public bool Update(IDestroyableObject owner, IAreaMapCell cell, IJournal journal)
        {
            if (burnTime >= burnBeforeExtinguishCheck)
            {
                if (RandomHelper.CheckChance(owner.GetSelfExtinguishChance()))
                {
                    return false;
                }
            }

            burnTime++;

            var damage = Temperature.GetTemperatureDamage(burningTemperature, out _);
            if (damage == 0)
                return true;

            journal.Write(new BurningDamageMessage(owner, damage));
            owner.Damage(journal, damage, Element.Fire);

            var temperatureDiff = cell.Temperature - burningTemperature;
            if (temperatureDiff > 0)
            {
                var cellTemperatureIncrement = Math.Min(temperatureDiff, CellTemperatureIncreaseMax);
                cell.Temperature += cellTemperatureIncrement;
            }

            return true;
        }

        public string Type => StatusType;
    }

    public class OnFireObjectStatusConfiguration
    {
        public OnFireObjectStatusConfiguration()
        {
            BurningTemperature = 600;
        }

        public int BurningTemperature { get; set; }
        public int BurnBeforeExtinguishCheck { get; set; }
    }
}