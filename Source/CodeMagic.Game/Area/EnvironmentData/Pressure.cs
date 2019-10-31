using System;
using System.Diagnostics;
using CodeMagic.Core.Configuration;

namespace CodeMagic.Game.Area.EnvironmentData
{
    [DebuggerDisplay("{" + nameof(Value) + "} kPa")]
    public class Pressure
    {
        private int value;
        private int oldValue;

        private readonly IPressureConfiguration configuration;

        public Pressure(IPressureConfiguration configuration)
        {
            this.configuration = configuration;

            value = configuration.NormalValue;
            oldValue = configuration.NormalValue;
        }

        public int Value
        {
            get => value;
            set
            {
                if (value < configuration.MinValue)
                {
                    this.value = configuration.MinValue;
                    return;
                }

                if (value > configuration.MaxValue)
                {
                    this.value = configuration.MaxValue;
                    return;
                }

                this.value = value;
            }
        }

        public void Normalize()
        {
            if (Value == configuration.NormalValue)
                return;

            var difference = Math.Abs(configuration.NormalValue - Value);
            difference = Math.Min(difference, configuration.NormalizeSpeed);

            if (Value > configuration.NormalValue)
            {
                Value -= difference;
            }

            if (Value < configuration.NormalValue)
            {
                Value += difference;
            }
        }

        public void Balance(Pressure other)
        {
            if (Value == other.Value)
                return;

            var medium = (int)Math.Round((Value + other.Value) / 2d);
            Value = medium;
            other.Value = medium;
        }

        public int GetPressureDamage()
        {
            var pureDamage = GetPurePressureDamage();
            var pressureChangeDamage = GetPressureChangeDamage();

            oldValue = Value;

            return pureDamage + pressureChangeDamage;
        }

        private int GetPressureChangeDamage()
        {
            var difference = Math.Abs(Value - oldValue);

            if (difference < configuration.ChangePressureDamageConfiguration.PressureLevel)
                return 0;

            var differenceValue = configuration.ChangePressureDamageConfiguration.PressureLevel - difference;
            return (int) Math.Round(differenceValue * configuration.ChangePressureDamageConfiguration.DamageMultiplier);
        }

        private int GetPurePressureDamage()
        {
            if (value < configuration.LowPressureDamageConfiguration.PressureLevel)
            {
                var diff = configuration.LowPressureDamageConfiguration.PressureLevel - value;
                return (int)Math.Round(diff * configuration.LowPressureDamageConfiguration.DamageMultiplier);
            }

            if (value > configuration.HighPressureDamageConfiguration.PressureLevel)
            {
                var diff = value - configuration.HighPressureDamageConfiguration.PressureLevel;
                return (int)Math.Round(diff * configuration.HighPressureDamageConfiguration.DamageMultiplier);
            }

            return 0;
        }
    }
}