using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Area
{
    public class AreaMapCell : AreaMapCellBase
    {
        public AreaMapCell(IEnvironment environment)
            : base(environment)
        {
        }

        public override bool HasRoof => ObjectsCollection.OfType<IRoof>().Any();

        public void Update(IAreaMap map, IJournal journal, Point position, UpdateOrder updateOrder)
        {
            ProcessDynamicObjects(map, journal, position, updateOrder);
        }

        public void PostUpdate(IAreaMap map, IJournal journal, Point position)
        {
            ProcessDestroyableObjects(map, journal, position);
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
                deadObject.OnDeath(map, journal, position);
            }
        }

        private void ProcessStatusesAndEnvironment(IDestroyableObject[] destroyableObjects, IJournal journal)
        {
            foreach (var destroyableObject in destroyableObjects)
            {
                destroyableObject.Statuses.Update(this, journal);

                if (destroyableObject is ICreatureObject && LightLevel == LightLevel.Blinding)
                {
                    destroyableObject.Statuses.Add(new BlindObjectStatus(), journal);
                }
            }
        }

        public void CheckSpreading(AreaMapCell other)
        {
            CheckSpreadingObjects(other);
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