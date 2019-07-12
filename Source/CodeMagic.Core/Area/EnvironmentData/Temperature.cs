using System;
using System.Diagnostics;
using System.Linq;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Area.EnvironmentData
{
    [DebuggerDisplay("{" + nameof(Value) + "} C")]
    public class Temperature
    {
        private int value;

        private readonly ITemperatureConfiguration configuration;

        public Temperature()
        {
            configuration = ConfigurationManager.Current.TemperatureConfiguration;
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

        public void Normalize()
        {
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
            if (value < 0)
            {
                foreach (var damageConfiguration in configuration.ColdDamageConfiguration.OrderBy(config => config.Temperature))
                {
                    if (value <= damageConfiguration.Temperature)
                    {
                        damageElement = Element.Frost;
                        return damageConfiguration.Damage;
                    }
                }
            }

            if (value > 0)
            {
                foreach (var damageConfiguration in configuration.HeatDamageConfiguration.OrderByDescending(config => config.Temperature))
                {
                    if (value >= damageConfiguration.Temperature)
                    {
                        damageElement = Element.Fire;
                        return damageConfiguration.Damage;
                    }
                }
            }

            damageElement = null;
            return 0;
        }
    }
}