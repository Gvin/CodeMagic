using System;
using System.Diagnostics;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Area
{
    [DebuggerDisplay("{Value} C")]
    public class Temperature
    {
        public const int WaterEvaporationTemperature = 100;
        public const int HeatDamageTemperature = WaterEvaporationTemperature;
        public const int WoodBurnTemperature = 600;
        public const int StoneMeltTemperature = 1200;
        public const int MetalMeltTemperature = 1500;
        public const int MaxTemperature = 3000;

        public const int WaterFreezingPoint = 0;

        public const int ColdDamageTemperature = -70;
        public const int FreezingTemperature = -100;
        public const int TotalFreezeTemperature = -200;

        public const int NormalTemperature = 25;

        public const int TransferValue = 10;

        public const int NormalizeTemperatureSpeed = 5;

        private int value;

        public Temperature()
        {
            value = NormalTemperature;
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
                if (value < TotalFreezeTemperature)
                {
                    this.value = TotalFreezeTemperature;
                    return;
                }

                if (value > MaxTemperature)
                {
                    this.value = MaxTemperature;
                    return;
                }

                this.value = value;
            }
        }

        public void Normalize()
        {
            var difference = Math.Abs(NormalTemperature - Value);
            difference = Math.Min(difference, NormalizeTemperatureSpeed);

            if (Value > NormalTemperature)
            {
                Value -= difference;
            }

            if (Value < NormalTemperature)
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
            difference = Math.Min(difference, TransferValue);

            if (Value > other.Value)
            {
                Value -= difference;
                other.Value += difference;
            }
            else
            {
                Value += difference;
                other.Value -= difference;
            }
        }

        public int GetTemperatureDamage(out Element? damageElement)
        {
            if (value <= TotalFreezeTemperature)
            {
                damageElement = Element.Frost;
                return 20;
            }

            if (value <= FreezingTemperature)
            {
                damageElement = Element.Frost;
                return 5;
            }

            if (value <= ColdDamageTemperature)
            {
                damageElement = Element.Frost;
                return 1;
            }

            if (value >= MetalMeltTemperature)
            {
                damageElement = Element.Fire;
                return 30;
            }

            if (value >= StoneMeltTemperature)
            {
                damageElement = Element.Fire;
                return 15;
            }

            if (value >= WoodBurnTemperature)
            {
                damageElement = Element.Fire;
                return 5;
            }

            if (value >= HeatDamageTemperature)
            {
                damageElement = Element.Fire;
                return 1;
            }

            damageElement = null;
            return 0;
        }
    }
}