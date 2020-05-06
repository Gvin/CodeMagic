using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Physics;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.Area.EnvironmentData
{
    public class MagicEnergy : ISaveable
    {
        private const string SaveKeyEnergy = "Energy";
        private const string SaveKeyDisturbance = "Disturbance";

        private int energy;
        private readonly IMagicEnergyConfiguration configuration;
        private int disturbance;

        public MagicEnergy(SaveData data)
        {
            configuration = ConfigurationManager.Current.Physics.MagicEnergyConfiguration;

            energy = data.GetIntValue(SaveKeyEnergy);
            disturbance = data.GetIntValue(SaveKeyDisturbance);
        }

        public MagicEnergy()
        {
            configuration = ConfigurationManager.Current.Physics.MagicEnergyConfiguration;

            energy = configuration.MaxValue;
            disturbance = 0;
        }

        public int MaxEnergy => configuration.MaxValue;

        public int Energy
        {
            get => energy;
            set
            {
                var clearValue = Math.Max(0, value);
                energy = Math.Min(CurrentMaxEnergy, clearValue);
            }
        }

        public int Disturbance
        {
            get => disturbance;
            set
            {
                value = Math.Max(0, value);
                disturbance = Math.Min(configuration.MaxValue, value);
            }
        }

        private int CurrentMaxEnergy => configuration.MaxValue - Disturbance;

        public void Normalize()
        {
            if (Energy > configuration.DisturbanceStartLevel)
            {
                Energy = Energy + configuration.RegenerationValue;
                return;
            }

            Disturbance = Math.Min(MaxEnergy, Disturbance + configuration.DisturbanceIncrement);
            Energy = Energy; // Refresh energy value.
        }

        public void Balance(MagicEnergy other)
        {
            if (other.Energy == other.CurrentMaxEnergy && Energy == CurrentMaxEnergy)
                return;

            var thisDiff = CurrentMaxEnergy - Energy;
            var otherDiff = other.CurrentMaxEnergy - other.Energy;

            if (thisDiff >= otherDiff)
            {
                var maxTransferValue = Math.Min(thisDiff, other.Energy);
                var transferValue = Math.Min(configuration.MaxTransferValue, maxTransferValue);

                Energy += transferValue;
                other.Energy -= transferValue;
                return;
            }

            {
                var maxTransferValue = Math.Min(otherDiff, Energy);
                var transferValue = Math.Min(configuration.MaxTransferValue, maxTransferValue);
                Energy -= transferValue;
                other.Energy += transferValue;
            }
        }

        public void ApplyMagicEnvironment(IDestroyableObject destroyable, Point position)
        {
            if (Disturbance > configuration.DisturbanceDamageStartLevel)
            {
                destroyable.Statuses.Add(new ManaDisturbedObjectStatus());
            }
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyEnergy, energy},
                {SaveKeyDisturbance, disturbance}
            });
        }
    }
}