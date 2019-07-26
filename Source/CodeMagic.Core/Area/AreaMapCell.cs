using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.SolidObjects;
using Environment = CodeMagic.Core.Area.EnvironmentData.Environment;

namespace CodeMagic.Core.Area
{
    public class AreaMapCell
    {
        private readonly LightLevel defaultLightLevel;

        public AreaMapCell(LightLevel defaultLightLevel)
        {
            this.defaultLightLevel = defaultLightLevel;

            Objects = new MapObjectsCollection();
            FloorType = FloorTypes.Stone;
            Environment = new Environment();
            LightLevel = defaultLightLevel;
        }

        public Environment Environment { get; }

        public MapObjectsCollection Objects { get; }

        public FloorTypes FloorType { get; set; }

        public LightLevel LightLevel { get; set; }

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

        public void ResetLightLevel()
        {
            LightLevel = defaultLightLevel;
        }

        public void Update(IGameCore game, Point position)
        {
            ProcessDynamicObjects(game, position);
            ProcessDestroyableObjects(game, position);

            Environment.Normalize();

            if (Environment.Temperature >= FireDecorativeObject.SmallFireTemperature && !Objects.OfType<FireDecorativeObject>().Any())
            {
                Objects.Add(MapObjectsFactory.CreateFire(Environment.Temperature));
            }
        }

        public void ResetDynamicObjectsState()
        {
            foreach (var dynamicObject in Objects.OfType<IDynamicObject>())
            {
                dynamicObject.Updated = false;
            }
        }

        private void ProcessDynamicObjects(IGameCore game, Point position)
        {
            var dynamicObjects = Objects.OfType<IDynamicObject>().Where(obj => !obj.Updated).ToArray();
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
            }
        }

        public void CheckSpreadingObjects(AreaMapCell other)
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