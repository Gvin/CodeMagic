using System;
using System.Diagnostics;
using System.Linq;
using CodeMagic.Core.Configuration;

namespace CodeMagic.Core.Area.EnvironmentData
{
    [DebuggerDisplay("{" + nameof(Value) + "} kPa")]
    public class Pressure
    {
        private int value;
        private int oldValue;

        private readonly IPressureConfiguration configuration;

        public Pressure()
        {
            configuration = ConfigurationManager.Current.PressureConfiguration;

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

            if (difference <= 0)
                return 0;

            foreach (var damageConfiguration in configuration.ChangePressureDamageConfiguration.OrderByDescending(config =>
                config.Pressure))
            {
                if (difference >= damageConfiguration.Pressure)
                    return damageConfiguration.Damage;
            }

            return 0;
        }

        private int GetPurePressureDamage()
        {
            if (value < configuration.NormalValue)
            {
                foreach (var damageConfiguration in configuration.LowPressureDamageConfiguration.OrderBy(config =>
                    config.Pressure))
                {
                    if (value <= damageConfiguration.Pressure)
                        return damageConfiguration.Damage;
                }
            }

            if (value > configuration.NormalValue)
            {
                foreach (var damageConfiguration in configuration.HighPressureDamageConfiguration.OrderByDescending(config =>
                    config.Pressure))
                {
                    if (value >= damageConfiguration.Pressure)
                        return damageConfiguration.Damage;
                }
            }

            return 0;
        }
    }
}