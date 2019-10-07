using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area.EnvironmentData
{
    public class Environment
    {
        private const double TemperatureToPressureMultiplier = 0.6d;

        private readonly Temperature temperature;
        private readonly Pressure pressure;

        public Environment()
        {
            temperature = new Temperature();
            pressure = new Pressure();
        }

        public int Temperature
        {
            get => temperature.Value;
            set
            {
                var oldValue = temperature.Value;
                temperature.Value = value;
                var diff = temperature.Value - oldValue;
                var pressureDiff = (int) Math.Round(diff * TemperatureToPressureMultiplier);
                Pressure += pressureDiff;
            }
        }

        public int Pressure
        {
            get => pressure.Value;
            set => pressure.Value = value;
        }

        public void Normalize(bool isInside)
        {
            temperature.Normalize(isInside);
            pressure.Normalize();
        }

        public void ApplyEnvironment(IDestroyableObject destroyable, IJournal journal)
        {
            var temperatureDamage = temperature.GetTemperatureDamage(out var temperDamageElement);
            var pressureDamage = pressure.GetPressureDamage();

            if (temperatureDamage > 0 && temperDamageElement.HasValue)
            {
                journal.Write(new EnvironmentDamageMessage(destroyable, temperatureDamage, temperDamageElement.Value));
                destroyable.Damage(journal, temperatureDamage, temperDamageElement.Value);
            }

            if (pressureDamage > 0)
            {
                journal.Write(new EnvironmentDamageMessage(destroyable, pressureDamage, Element.Blunt));
                destroyable.Damage(journal, pressureDamage, Element.Blunt);
            }
        }

        public void Balance(Environment otherEnvironment)
        {
            temperature.Balance(otherEnvironment.temperature);
            pressure.Balance(otherEnvironment.pressure);
        }
    }
}