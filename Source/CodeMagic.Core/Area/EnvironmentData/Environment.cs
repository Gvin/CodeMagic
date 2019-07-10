using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area.EnvironmentData
{
    public class Environment
    {
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
            set => temperature.Value = value;
        }

        public int Pressure
        {
            get => pressure.Value;
            set => pressure.Value = value;
        }

        public void Normalize()
        {
            temperature.Normalize();
            pressure.Normalize();
        }

        public void ApplyEnvironment(IDestroyableObject[] objects, Journal journal)
        {
            var temperatureDamage = temperature.GetTemperatureDamage(out var temperDamageElement);
            var pressureDamage = pressure.GetPressureDamage();

            foreach (var destroyableObject in objects)
            {
                if (temperatureDamage > 0)
                {
                    journal.Write(new EnvironmentDamageMessage(destroyableObject, temperatureDamage, temperDamageElement));
                    destroyableObject.Damage(temperatureDamage, temperDamageElement);
                }

                if (pressureDamage > 0)
                {
                    journal.Write(new EnvironmentDamageMessage(destroyableObject, pressureDamage));
                    destroyableObject.Damage(pressureDamage);
                }
            }
        }

        public void Balance(Environment otherEnvironment)
        {
            temperature.Balance(otherEnvironment.temperature);
            pressure.Balance(otherEnvironment.pressure);
        }
    }
}