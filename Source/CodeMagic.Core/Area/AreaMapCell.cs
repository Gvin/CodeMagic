using System;
using System.Linq;
using CodeMagic.Core.Area.EnvironmentData;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Statuses;
using Environment = CodeMagic.Core.Area.EnvironmentData.Environment;

namespace CodeMagic.Core.Area
{
    public class AreaMapCell : AreaMapCellBase
    {
        public AreaMapCell(IMagicEnergyConfiguration magicEnergyConfiguration)
        {
            Environment = new Environment();
            MagicEnergy = new MagicEnergy(magicEnergyConfiguration);
        }

        public MagicEnergy MagicEnergy { get; }

        public Environment Environment { get; }

        public override int Temperature
        {
            get => Environment.Temperature;
            set => Environment.Temperature = value;
        }

        public override int Pressure
        {
            get => Environment.Pressure;
            set => Environment.Pressure = value;
        }

        public override int MagicEnergyLevel
        {
            get => MagicEnergy.Energy;
            set => MagicEnergy.Energy = value;
        }

        public override int MaxMagicEnergyLevel => MagicEnergy.MaxEnergy;

        public override int MagicDisturbanceLevel
        {
            get => MagicEnergy.Disturbance;
            set => MagicEnergy.Disturbance = value;
        }

        public override bool HasRoof => ObjectsCollection.OfType<IRoofObject>().Any();

        public void Update(IAreaMap map, IJournal journal, Point position, UpdateOrder updateOrder)
        {
            ProcessDynamicObjects(map, journal, position, updateOrder);
        }

        public void PostUpdate(IAreaMap map, IJournal journal, Point position)
        {
            ProcessDestroyableObjects(map, journal, position);
        }

        public void UpdateEnvironment(IAreaMap map, Point position)
        {
            CheckFuelObjects(map, position);

            var isInside = ObjectsCollection.OfType<IRoofObject>().Any();

            Environment.Normalize(isInside);

            if (Environment.Temperature >= FireDecorativeObject.SmallFireTemperature && !ObjectsCollection.OfType<FireDecorativeObject>().Any())
            {
                ObjectsCollection.Add(Injector.Current.Create<IFireDecorativeObject>(Environment.Temperature));
            }

            MagicEnergy.Update();
        }

        public void ResetDynamicObjectsState()
        {
            foreach (var dynamicObject in ObjectsCollection.OfType<IDynamicObject>())
            {
                dynamicObject.Updated = false;
            }
        }

        private void ProcessDynamicObjects(IAreaMap map, IJournal journal, Point position, UpdateOrder updateOrder)
        {
            var dynamicObjects = ObjectsCollection.OfType<IDynamicObject>()
                .Where(obj => !obj.Updated && obj.UpdateOrder == updateOrder).ToArray();
            foreach (var dynamicObject in dynamicObjects)
            {
                dynamicObject.Update(map, journal, position);
                dynamicObject.Updated = true;
            }
        }

        private void ProcessDestroyableObjects(IAreaMap map, IJournal journal, Point position)
        {
            var destroyableObjects = ObjectsCollection.OfType<IDestroyableObject>().ToArray();
            ProcessStatusesAndEnvironment(destroyableObjects, journal);

            var deadObjects = destroyableObjects.Where(obj => obj.Health <= 0).ToArray();
            foreach (var deadObject in deadObjects)
            {
                map.RemoveObject(position, deadObject);
                journal.Write(new DeathMessage(deadObject));
                deadObject.OnDeath(map, journal, position);
            }
        }

        private void ProcessStatusesAndEnvironment(IDestroyableObject[] destroyableObjects, IJournal journal)
        {
            foreach (var destroyableObject in destroyableObjects)
            {
                destroyableObject.Statuses.Update(this, journal);

                Environment.ApplyEnvironment(destroyableObject, journal);
                MagicEnergy.ApplyMagicEnvironment(destroyableObject, journal);

                if (destroyableObject is ICreatureObject && LightLevel.IsBlinding)
                {
                    destroyableObject.Statuses.Add(new BlindObjectStatus(), journal);
                }
            }
        }

        public void CheckSpreading(AreaMapCell other)
        {
            CheckSpreadingObjects(other);
            CheckFireSpread(other);
        }

        private void CheckFuelObjects(IAreaMap map, Point position)
        {
            var fuelObjects = ObjectsCollection
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

        private void CheckFireSpread(AreaMapCell other)
        {
            var localIgnitable = ObjectsCollection.OfType<IFireSpreadingObject>().FirstOrDefault(obj => obj.SpreadsFire);
            var otherIgnitable = ObjectsCollection.OfType<IFireSpreadingObject>().FirstOrDefault(obj => obj.SpreadsFire);

            if (localIgnitable == null || otherIgnitable == null)
                return;

            if (localIgnitable.GetIsOnFire(this) && other.Environment.Temperature < localIgnitable.BurningTemperature)
            {
                other.Environment.Temperature = localIgnitable.BurningTemperature;
            }

            if (otherIgnitable.GetIsOnFire(other) && Environment.Temperature < otherIgnitable.BurningTemperature)
            {
                Environment.Temperature = otherIgnitable.BurningTemperature;
            }
        }

        private void CheckSpreadingObjects(AreaMapCell other)
        {
            var localSpreadingObjects = ObjectsCollection.OfType<ISpreadingObject>().ToArray();
            var otherSpreadingObjects = other.ObjectsCollection.OfType<ISpreadingObject>().ToArray();

            foreach (var spreadingObject in localSpreadingObjects)
            {
                if (spreadingObject.Volume >= spreadingObject.MaxVolumeBeforeSpread)
                {
                    SpreadObject(spreadingObject, other);
                }
            }

            foreach (var otherSpreadingObject in otherSpreadingObjects)
            {
                if (otherSpreadingObject.Volume >= otherSpreadingObject.MaxVolumeBeforeSpread)
                {
                    SpreadObject(otherSpreadingObject, other);
                }
            }
        }

        private void SpreadObject(ISpreadingObject liquid, AreaMapCell target)
        {
            var spreadAmount = Math.Min(liquid.MaxSpreadVolume, liquid.Volume - liquid.MaxVolumeBeforeSpread);
            var separated = liquid.Separate(spreadAmount);
            target.ObjectsCollection.AddVolumeObject(separated);
        }
    }
}