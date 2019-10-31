using System;
using System.Diagnostics;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;

namespace CodeMagic.Game.Area.EnvironmentData
{
    [DebuggerDisplay("{" + nameof(Value) + "} C")]
    public class Temperature
    {
        private int value;

        private readonly ITemperatureConfiguration configuration;

        public Temperature(ITemperatureConfiguration configuration)
        {
            this.configuration = configuration;
            value = configuration.NormalValue;
        }

        public Temperature(int value)
        {
            Value = value;
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

        public void Normalize(bool isInside)
        {
            var difference = Math.Abs(configuration.NormalValue - Value);
            difference = Math.Min(difference, isInside ? configuration.NormalizeSpeedInside : configuration.NormalizeSpeedOutside);

            if (Value > configuration.NormalValue)
            {
                Value -= difference;
            }

            if (Value < configuration.NormalValue)
            {
                Value += difference;
            }
        }

        public void Balance(Temperature other)
        {
            if (Value == other.Value)
                return;

            var mediana = (int)Math.Round((Value + other.Value) / 2d);
            var difference = Math.Abs(Value - mediana);
            var transferValue = GetTemperatureTransferValue(difference);

            if (Value > other.Value)
            {
                Value -= transferValue;
                other.Value += transferValue;
            }
            else
            {
                Value += transferValue;
                other.Value -= transferValue;
            }
        }

        private int GetTemperatureTransferValue(int difference)
        {
            var result = (int)Math.Round(difference * configuration.TransferValueToDifferenceMultiplier);
            return Math.Min(result, configuration.MaxTransferValue);
        }

        public int GetTemperatureDamage(out Element? damageElement)
        {
            return GetTemperatureDamage(configuration, value, out damageElement);
        }

        public static int GetTemperatureDamage(ITemperatureConfiguration configuration, int temperature, out Element? damageElement)
        {
            if (temperature < configuration.ColdDamageConfiguration.TemperatureLevel)
            {

                damageElement = Element.Frost;
                var damageValue = configuration.ColdDamageConfiguration.TemperatureLevel - temperature;
                return (int)Math.Round(damageValue * configuration.ColdDamageConfiguration.DamageMultiplier);
            }

            if (temperature > configuration.HeatDamageConfiguration.TemperatureLevel)
            {

                damageElement = Element.Fire;
                var damageValue = temperature - configuration.HeatDamageConfiguration.TemperatureLevel;
                return (int)Math.Round(damageValue * configuration.HeatDamageConfiguration.DamageMultiplier);
            }

            damageElement = null;
            return 0;
        }
    }
}