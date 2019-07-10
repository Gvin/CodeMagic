using System;
using CodeMagic.Core.Area;
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
        private readonly int fireDamageMin;
        private readonly int fireDamageMax;
        private readonly int burnBeforeExtinguishCheck;

        private int burnTime = 0;

        public OnFireObjectStatus(OnFireObjectStatusConfiguration configuration)
        {
            burningTemperature = configuration.BurningTemperature;
            fireDamageMin = configuration.FireDamageMin;
            fireDamageMax = configuration.FireDamageMax;
            burnBeforeExtinguishCheck = configuration.BurnBeforeExtinguishCheck;
        }

        public bool Update(IDestroyableObject owner, AreaMapCell cell, Journal journal)
        {
            if (burnTime >= burnBeforeExtinguishCheck)
            {
                if (RandomHelper.CheckChance(owner.SelfExtinguishChance))
                {
                    return false;
                }
            }

            burnTime++;

            var damage = RandomHelper.GetRandomValue(fireDamageMin, fireDamageMax);
            journal.Write(new BurningDamageMessage(owner, damage));
            owner.Damage(damage, Element.Fire);

            var temperatureDiff = cell.Environment.Temperature - burningTemperature;
            if (temperatureDiff > 0)
            {
                var cellTemperatureIncrement = Math.Min(temperatureDiff, CellTemperatureIncreaseMax);
                cell.Environment.Temperature += cellTemperatureIncrement;
            }

            return true;
        }

        public string Type => StatusType;
    }

    public class OnFireObjectStatusConfiguration
    {
        public int BurningTemperature { get; set; }
        public int FireDamageMin { get; set; }
        public int FireDamageMax { get; set; }
        public int BurnBeforeExtinguishCheck { get; set; }
    }
}