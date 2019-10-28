using System.Linq;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public class GlobalAreaMapCell : AreaMapCellBase
    {
        private readonly IPhysicsConfiguration configuration;

        public GlobalAreaMapCell(IPhysicsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override bool HasRoof => false;

        public override int Temperature
        {
            get => configuration.TemperatureConfiguration.NormalValue;
            set { }
        }

        public override int Pressure
        {
            get => configuration.PressureConfiguration.NormalValue;
            set { }
        }

        public override int MagicEnergyLevel
        {
            get => int.MaxValue;
            set { }
        }

        public override int MaxMagicEnergyLevel => int.MaxValue;

        public override int MagicDisturbanceLevel
        {
            get => 0;
            set { }
        }

        public void Update(IAreaMap map, IJournal journal, Point position, UpdateOrder updateOrder)
        {
            ProcessDynamicObjects(map, journal, position, updateOrder);
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

        public void ResetDynamicObjectsState()
        {
            foreach (var dynamicObject in ObjectsCollection.OfType<IDynamicObject>())
            {
                dynamicObject.Updated = false;
            }
        }

        public void PostUpdate(IAreaMap map, IJournal journal, Point position)
        {
            ProcessDestroyableObjects(map, journal, position);
        }

        private void ProcessDestroyableObjects(IAreaMap map, IJournal journal, Point position)
        {
            var destroyableObjects = ObjectsCollection.OfType<IDestroyableObject>().ToArray();
            ProcessStatuses(destroyableObjects, journal);

            var deadObjects = destroyableObjects.Where(obj => obj.Health <= 0).ToArray();
            foreach (var deadObject in deadObjects)
            {
                map.RemoveObject(position, deadObject);
                journal.Write(new DeathMessage(deadObject));
                deadObject.OnDeath(map, journal, position);
            }
        }

        private void ProcessStatuses(IDestroyableObject[] destroyableObjects, IJournal journal)
        {
            foreach (var destroyableObject in destroyableObjects)
            {
                destroyableObject.Statuses.Update(this, journal);
            }
        }
    }
}