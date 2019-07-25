using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.Core.Objects.SolidObjects;
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
        }

        public Environment Environment { get; }

        public MapObjectsCollection Objects { get; }

        public FloorTypes FloorType { get; set; }

        public bool BlocksMovement
        {
            get { return Objects.Any(obj => obj.BlocksMovement); }
        }

        public bool HasSolidObjects => Objects.OfType<SolidObject>().Any();

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
                Objects.Remove(deadObject);
                game.Map.UnregisterDestroyableObject(deadObject);
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

        public void CheckLiquidSpreading(AreaMapCell other)
        {
            var localLiquids = Objects.OfType<ILiquidObject>().ToArray();
            var otherLiquids = Objects.OfType<ILiquidObject>().ToArray();

            foreach (var liquid in localLiquids)
            {
                if (liquid.Volume >= liquid.MaxVolumeBeforeSpread)
                {
                    SpreadLiquid(liquid, other);
                }
            }

            foreach (var otherLiquid in otherLiquids)
            {
                if (otherLiquid.Volume >= otherLiquid.MaxVolumeBeforeSpread)
                {
                    SpreadLiquid(otherLiquid, other);
                }
            }
        }

        private void SpreadLiquid(ILiquidObject liquid, AreaMapCell target)
        {
            var spreadAmount = Math.Min(liquid.MaxSpreadVolume, liquid.Volume - liquid.MaxVolumeBeforeSpread);
            var separated = liquid.Separate(spreadAmount);
            target.Objects.AddLiquid(separated);
        }
    }
}