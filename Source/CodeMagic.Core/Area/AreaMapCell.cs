using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Core.Statuses;
using Environment = CodeMagic.Core.Area.EnvironmentData.Environment;

namespace CodeMagic.Core.Area
{
    public class AreaMapCell
    {
        public AreaMapCell()
        {
            Objects = new MapObjectsCollection();
            FloorType = FloorTypes.Stone;
            Environment = new Environment();

            LightLevel = new CellLightData();
        }

        public Environment Environment { get; }

        public MapObjectsCollection Objects { get; }

        public FloorTypes FloorType { get; set; }

        public CellLightData LightLevel { get; set; }

        public bool BlocksMovement
        {
            get { return Objects.Any(obj => obj.BlocksMovement); }
        }

        public bool HasSolidObjects => Objects.OfType<WallObject>().Any();

        public bool BlocksEnvironment
        {
            get { return Objects.Any(obj => obj.BlocksEnvironment); }
        }

        public bool BlocksVisibility
        {
            get { return Objects.Any(obj => obj.BlocksVisibility); }
        }

        public bool BlocksProjectiles
        {
            get { return Objects.Any(obj => obj.BlocksProjectiles); }
        }

        public IDestroyableObject GetBiggestDestroyable()
        {
            var destroyable = Objects.OfType<IDestroyableObject>().ToArray();
            var bigDestroyable = destroyable.FirstOrDefault(obj => obj.BlocksMovement);
            if (bigDestroyable != null)
                return bigDestroyable;
            return destroyable.LastOrDefault();
        }

        public void Update(IGameCore game, Point position, UpdateOrder updateOrder)
        {
            ProcessDynamicObjects(game, position, updateOrder);
        }

        public void PostUpdate(IGameCore game, Point position)
        {
            ProcessDestroyableObjects(game, position);
        }

        public void UpdateEnvironment()
        {
            Environment.Normalize();

            if (Environment.Temperature >= FireDecorativeObject.SmallFireTemperature && !Objects.OfType<FireDecorativeObject>().Any())
            {
                Objects.Add(Injector.Current.Create<IFireDecorativeObject>(Environment.Temperature));
            }
        }

        public void ResetDynamicObjectsState()
        {
            foreach (var dynamicObject in Objects.OfType<IDynamicObject>())
            {
                dynamicObject.Updated = false;
            }
        }

        private void ProcessDynamicObjects(IGameCore game, Point position, UpdateOrder updateOrder)
        {
            var dynamicObjects = Objects.OfType<IDynamicObject>()
                .Where(obj => !obj.Updated && obj.UpdateOrder == updateOrder).ToArray();
            foreach (var dynamicObject in dynamicObjects)
            {
                dynamicObject.Update(game, position);
                dynamicObject.Updated = true;
            }
        }

        private void ProcessDestroyableObjects(IGameCore game, Point position)
        {
            var destroyableObjects = Objects.OfType<IDestroyableObject>().ToArray();
            ProcessStatusesAndEnvironment(destroyableObjects, game.Journal);

            var deadObjects = destroyableObjects.Where(obj => obj.Health <= 0).ToArray();
            foreach (var deadObject in deadObjects)
            {
                game.Map.RemoveObject(position, deadObject);
                game.Journal.Write(new DeathMessage(deadObject));
                deadObject.OnDeath(game.Map, position);
            }
        }

        private void ProcessStatusesAndEnvironment(IDestroyableObject[] destroyableObjects, Journal journal)
        {
            foreach (var destroyableObject in destroyableObjects)
            {
                destroyableObject.Statuses.Update(destroyableObject, this, journal);
                Environment.ApplyEnvironment(destroyableObject, journal);
                if (destroyableObject is ICreatureObject && LightLevel.IsBlinding)
                {
                    destroyableObject.Statuses.Add(new BlindObjectStatus());
                }
            }
        }

        public void CheckSpreading(AreaMapCell other)
        {
            CheckSpreadingObjects(other);
            CheckFireSpread(other);
        }

        private void CheckFireSpread(AreaMapCell other)
        {
            var localIgnitable = Objects.OfType<IFireSpreadingObject>().FirstOrDefault(obj => obj.SpreadsFire);
            var otherIgnitable = Objects.OfType<IFireSpreadingObject>().FirstOrDefault(obj => obj.SpreadsFire);

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
            var localSpreadingObjects = Objects.OfType<ISpreadingObject>().ToArray();
            var otherSpreadingObjects = other.Objects.OfType<ISpreadingObject>().ToArray();

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
            target.Objects.AddVolumeObject(separated);
        }
    }
}