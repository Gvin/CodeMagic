using System;

namespace CodeMagic.Core.Area.EnvironmentData
{
    public class Pressure
    {
        private const int MinPressure = 0;
        private const int MaxPressure = 4000;

        private const int NormalPressure = 100;

        private const int SmallDamagePressure = 600;
        private const int MediumDamagePressure = 1300;
        private const int HighDamagePressure = 2000;

        private const int NormalizePressureSpeed = 50;

        private const int SmallDamage = 1;
        private const int MediumDamage = 5;
        private const int HighDamage = 10;

        private const int SmallDamagePressureChange = 300;
        private const int MediumDamagePressureChange = 900;
        private const int HighDamagePressureChange = 1500;

        private const int SmallDamageChange = 10;
        private const int MediumDamageChange = 30;
        private const int HighDamageChange = 70;

        private int value;
        private int oldValue;

        public Pressure()
        {
            value = NormalPressure;
            oldValue = NormalPressure;
        }

        public int Value
        {
            get => value;
            set
            {
                if (value < MinPressure)
                {
                    this.value = MinPressure;
                    return;
                }

                if (value > MaxPressure)
                {
                    this.value = MaxPressure;
                    return;
                }

                this.value = value;
            }
        }

        public void Normalize()
        {
            if (Value == NormalPressure)
                return;

            var difference = Math.Abs(NormalPressure - Value);
            difference = Math.Min(difference, NormalizePressureSpeed);

            if (Value > NormalPressure)
            {
                Value -= difference;
            }

            if (Value < NormalPressure)
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
            if (difference >= HighDamagePressureChange)
                return HighDamageChange;
            if (difference >= MediumDamagePressureChange)
                return MediumDamageChange;
            if (difference >= SmallDamagePressureChange)
                return SmallDamageChange;

            return 0;
        }

        private int GetPurePressureDamage()
        {
            if (Value >= HighDamagePressure)
                return HighDamage;
            if (Value >= MediumDamagePressure)
                return MediumDamage;
            if (Value >= SmallDamagePressure)
                return SmallDamage;

            return 0;
        }
    }
}