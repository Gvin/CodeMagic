using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Configuration.Physics;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Area.EnvironmentData
{
    public class GameEnvironment : IGameEnvironment
    {
        private const double TemperatureToPressureMultiplier = 0.6d;

        private readonly Temperature temperature;
        private readonly Pressure pressure;
        private readonly MagicEnergy magicEnergy;

        public GameEnvironment(IPhysicsConfiguration configuration)
        {
            temperature = new Temperature(configuration.TemperatureConfiguration);
            pressure = new Pressure(configuration.PressureConfiguration);
            magicEnergy = new MagicEnergy(configuration.MagicEnergyConfiguration);
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

        public int MagicEnergyLevel
        {
            get => magicEnergy.Energy;
            set => magicEnergy.Energy = value;
        }

        public int MaxMagicEnergyLevel => magicEnergy.MaxEnergy;

        public int MagicDisturbanceLevel
        {
            get => magicEnergy.Disturbance;
            set => magicEnergy.Disturbance = value;
        }

        public int Pressure
        {
            get => pressure.Value;
            set => pressure.Value = value;
        }

        public void Update(IAreaMap map, Point position, IAreaMapCell cell, IJournal journal)
        {
            CheckFuelObjects(map, position, cell);

            foreach (var destroyableObject in cell.Objects.OfType<IDestroyableObject>())
            {
                ApplyEnvironment(destroyableObject, journal);
            }

            Normalize(cell.Objects.OfType<IRoof>().Any());

            if (temperature.Value >= FireObject.SmallFireTemperature && !cell.Objects.OfType<FireObject>().Any())
            {
                map.AddObject(position, new FireObject(temperature.Value));
            }
        }

        private void Normalize(bool isInside)
        {
            temperature.Normalize(isInside);
            pressure.Normalize();
            magicEnergy.Normalize();
        }

        private void CheckFuelObjects(IAreaMap map, Point position, IAreaMapCell cell)
        {
            var fuelObjects = cell.Objects
                .OfType<IFuelObject>()
                .Where(obj => obj.CanIgnite && Temperature >= obj.IgnitionTemperature)
                .ToArray();
            if (fuelObjects.Length == 0)
                return;

            var maxTemperature = fuelObjects.Max(obj => obj.BurnTemperature);
            Temperature = maxTemperature;
            foreach (var fuelObject in fuelObjects)
            {
                fuelObject.FuelLeft--;
                if (fuelObject.FuelLeft <= 0)
                {
                    map.RemoveObject(position, fuelObject);
                }
            }
        }

        private void ApplyEnvironment(IDestroyableObject destroyable, IJournal journal)
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

            magicEnergy.ApplyMagicEnvironment(destroyable, journal);
        }

        public void Balance(IAreaMapCell cell, IAreaMapCell otherCell)
        {
            var otherEnvironment = (GameEnvironment)otherCell.Environment;

            temperature.Balance(otherEnvironment.temperature);
            pressure.Balance(otherEnvironment.pressure);
            magicEnergy.Balance(otherEnvironment.magicEnergy);

            CheckFireSpread(cell, otherCell);
        }

        private void CheckFireSpread(IAreaMapCell cell, IAreaMapCell otherCell)
        {
            var localEnvironment = (GameEnvironment) cell.Environment;
            var otherEnvironment = (GameEnvironment) otherCell.Environment;

            var localIgnitable = cell.Objects.OfType<IFireSpreadingObject>().FirstOrDefault(obj => obj.SpreadsFire);
            var otherIgnitable = otherCell.Objects.OfType<IFireSpreadingObject>().FirstOrDefault(obj => obj.SpreadsFire);

            if (localIgnitable == null || otherIgnitable == null)
                return;

            if (localIgnitable.GetIsOnFire(cell) && otherEnvironment.Temperature < localIgnitable.BurningTemperature)
            {
                otherEnvironment.Temperature = Math.Max(otherEnvironment.Temperature, localIgnitable.BurningTemperature);
            }

            if (otherIgnitable.GetIsOnFire(otherCell) && localEnvironment.Temperature < otherIgnitable.BurningTemperature)
            {
                localEnvironment.Temperature = Math.Max(localEnvironment.Temperature, otherIgnitable.BurningTemperature);
            }
        }
    }
}