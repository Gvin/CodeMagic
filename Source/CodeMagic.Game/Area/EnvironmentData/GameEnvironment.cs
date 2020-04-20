using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Area.EnvironmentData
{
    public class GameEnvironment : IGameEnvironment
    {
        private const string SaveKeyTemperature = "Temperature";
        private const string SaveKeyPressure = "Pressure";
        private const string SaveKeyMagicEnergy = "MagicEnergy";

        private const double TemperatureToPressureMultiplier = 0.6d;

        private readonly Temperature temperature;
        private readonly Pressure pressure;
        private readonly MagicEnergy magicEnergy;

        public GameEnvironment(SaveData data)
        {
            temperature = data.GetObject<Temperature>(SaveKeyTemperature);
            pressure = data.GetObject<Pressure>(SaveKeyPressure);
            magicEnergy = data.GetObject<MagicEnergy>(SaveKeyMagicEnergy);
        }

        public GameEnvironment()
        {
            temperature = new Temperature();
            pressure = new Pressure();
            magicEnergy = new MagicEnergy();
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyTemperature, temperature},
                {SaveKeyPressure, pressure},
                {SaveKeyMagicEnergy, magicEnergy}
            });
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

        public void Update(Point position, IAreaMapCell cell)
        {
            CheckFuelObjects(position, cell);

            foreach (var destroyableObject in cell.Objects.OfType<IDestroyableObject>())
            {
                ApplyEnvironment(destroyableObject, position);
            }

            Normalize();

            if (temperature.Value >= FireObject.SmallFireTemperature && !cell.Objects.OfType<FireObject>().Any())
            {
                CurrentGame.Map.AddObject(position, new FireObject(temperature.Value));
            }
        }

        private void Normalize()
        {
            temperature.Normalize();
            pressure.Normalize();
            magicEnergy.Normalize();
        }

        private void CheckFuelObjects(Point position, IAreaMapCell cell)
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
                    CurrentGame.Map.RemoveObject(position, fuelObject);
                }
            }
        }

        private void ApplyEnvironment(IDestroyableObject destroyable, Point position)
        {
            var temperatureDamage = temperature.GetTemperatureDamage(out var temperDamageElement);
            var pressureDamage = pressure.GetPressureDamage();

            if (temperatureDamage > 0 && temperDamageElement.HasValue)
            {
                CurrentGame.Journal.Write(new EnvironmentDamageMessage(destroyable, temperatureDamage, temperDamageElement.Value));
                destroyable.Damage(position, temperatureDamage, temperDamageElement.Value);
            }

            if (pressureDamage > 0)
            {
                CurrentGame.Journal.Write(new EnvironmentDamageMessage(destroyable, pressureDamage, Element.Blunt));
                destroyable.Damage(position, pressureDamage, Element.Blunt);
            }

            magicEnergy.ApplyMagicEnvironment(destroyable, position);
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