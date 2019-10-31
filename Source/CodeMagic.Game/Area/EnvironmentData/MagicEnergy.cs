using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Configuration.Physics;

namespace CodeMagic.Game.Area.EnvironmentData
{
    public class MagicEnergy
    {
        private int energy;
        private readonly IMagicEnergyConfiguration configuration;
        private int disturbance;

        public MagicEnergy(IMagicEnergyConfiguration configuration)
        {
            this.configuration = configuration;

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

        public void ApplyMagicEnvironment(IDestroyableObject destroyable, IJournal journal)
        {
            var damage = CalculateDamage();
            if (damage <= 0)
                return;

            destroyable.Damage(journal, damage, Element.Magic);
            journal.Write(new EnvironmentDamageMessage(destroyable, damage, Element.Magic));
        }

        private int CalculateDamage()
        {
            var disturbanceOverLimit = Disturbance - configuration.DisturbanceDamageStartLevel;
            if (disturbanceOverLimit <= 0)
                return 0;

            return (int) Math.Floor(Disturbance * configuration.DisturbanceDamageMultiplier);
        }
    }
}