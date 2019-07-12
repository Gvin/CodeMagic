using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area.EnvironmentData;
using CodeMagic.Core.Area.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.Core.Area
{
    public class AreaMapCell
    {
        public AreaMapCell()
        {
            Objects = new List<IMapObject>();
            FloorType = FloorTypes.Stone;
            Environment = new Environment();
            Liquids = new LiquidsData();
        }

        public Environment Environment { get; }

        public LiquidsData Liquids { get; }

        public List<IMapObject> Objects { get; }

        public FloorTypes FloorType { get; set; }

        public bool BlocksMovement
        {
            get { return Objects.Any(obj => obj.BlocksMovement); }
        }

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
            Liquids.UpdateLiquids(this);

            if (Environment.Temperature >= FireDecorativeObject.SmallFireTemperature && !Objects.OfType<FireDecorativeObject>().Any())
            {
                Objects.Add(new FireDecorativeObject(Environment.Temperature));
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
                Liquids.ApplyLiquids(destroyableObject);
            }
        }
    }
}