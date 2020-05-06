﻿using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.JournalMessages;

namespace CodeMagic.Game.Statuses
{
    public class OnFireObjectStatus : IObjectStatus
    {
        private const string SaveKeyBurningTemperature = "BurningTemperature";
        private const string SaveKeyBurnBeforeExtinguishCheck = "BurnBeforeExtinguishCheck";
        private const string SaveKeyBurnTime = "BurnTime";

        private const int CellTemperatureIncreaseMax = 100;

        public const string StatusType = "on_fire";

        private readonly int burningTemperature;
        private readonly int burnBeforeExtinguishCheck;

        private int burnTime;

        public OnFireObjectStatus(SaveData data)
        {
            burningTemperature = data.GetIntValue(SaveKeyBurningTemperature);
            burnBeforeExtinguishCheck = data.GetIntValue(SaveKeyBurnBeforeExtinguishCheck);
            burnTime = data.GetIntValue(SaveKeyBurnTime);
        }

        public OnFireObjectStatus(OnFireObjectStatusConfiguration configuration)
        {
            burningTemperature = configuration.BurningTemperature;
            burnBeforeExtinguishCheck = configuration.BurnBeforeExtinguishCheck;

            burnTime = 0;
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyBurnBeforeExtinguishCheck, burnBeforeExtinguishCheck},
                {SaveKeyBurningTemperature, burningTemperature},
                {SaveKeyBurnTime, burnTime}
            });
        }

        public bool Update(IDestroyableObject owner, Point position)
        {
            if (burnTime >= burnBeforeExtinguishCheck)
            {
                if (RandomHelper.CheckChance((int)Math.Round(owner.GetSelfExtinguishChance())))
                {
                    return false;
                }
            }

            burnTime++;

            var damage = Temperature.GetTemperatureDamage(ConfigurationManager.Current.Physics.TemperatureConfiguration, burningTemperature, out _);
            if (damage == 0)
                return true;

            CurrentGame.Journal.Write(new BurningDamageMessage(owner, damage), owner);
            owner.Damage(position, damage, Element.Fire);

            var cell = CurrentGame.Map.GetCell(position);
            var temperatureDiff = cell.Temperature() - burningTemperature;
            if (temperatureDiff > 0)
            {
                var cellTemperatureIncrement = Math.Min(temperatureDiff, CellTemperatureIncreaseMax);
                cell.Environment.Cast().Temperature += cellTemperatureIncrement;
            }

            return true;
        }

        public IObjectStatus Merge(IObjectStatus oldStatus)
        {
            if (!(oldStatus is OnFireObjectStatus oldFire))
                throw new InvalidOperationException($"Unable to merge {nameof(OnFireObjectStatus)} status with {oldStatus.GetType().Name}");

            return oldFire;
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