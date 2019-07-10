using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area.EnvironmentData;
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
        }

        public Environment Environment { get; }

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

        public void Update(IAreaMap map, Point position, Journal journal)
        {
            ProcessDynamicObjects(map, position, journal);
            ProcessDestroyableObjects(map, position, journal);

            Environment.Normalize();

            if (Environment.Temperature >= Temperature.WoodBurnTemperature && !Objects.OfType<FireDecorativeObject>().Any())
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

        private void ProcessDynamicObjects(IAreaMap map, Point position, Journal journal)
        {
            var dynamicObjects = Objects.OfType<IDynamicObject>().Where(obj => !obj.Updated).ToArray();
            foreach (var dynamicObject in dynamicObjects)
            {
                dynamicObject.Update(map, position, journal);
                dynamicObject.Updated = true;
            }
        }

        private void ProcessDestroyableObjects(IAreaMap map, Point position, Journal journal)
        {
            var destroyableObjects = Objects.OfType<IDestroyableObject>().ToArray();
            ProcessStatuses(destroyableObjects, journal);
            Environment.ApplyEnvironment(destroyableObjects, journal);

            var deadObjects = destroyableObjects.Where(obj => obj.Health <= 0).ToArray();
            foreach (var deadObject in deadObjects)
            {
                Objects.Remove(deadObject);
                map.UnregisterDestroyableObject(deadObject);
                journal.Write(new DeathMessage(deadObject));
                deadObject.OnDeath(map, position);
            }
        }

        private void ProcessStatuses(IDestroyableObject[] destroyableObjects, Journal journal)
        {
            foreach (var destroyableObject in destroyableObjects)
            {
                destroyableObject.Statuses.Update(destroyableObject, this, journal);
            }
        }
    }
}