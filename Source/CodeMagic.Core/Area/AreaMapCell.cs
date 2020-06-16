using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Area
{
    public class AreaMapCell : AreaMapCellBase, IAreaMapCellInternal
    {
        public AreaMapCell(SaveData data)
            : base(data)
        {
        }

        public AreaMapCell(IEnvironment environment)
            : base(environment)
        {
        }

        public void Update(Point position, UpdateOrder updateOrder)
        {
            ProcessDynamicObjects(position, updateOrder);
        }

        public void PostUpdate(IAreaMap map, Point position)
        {
            ProcessDestroyableObjects(map, position);
        }

        public void ResetDynamicObjectsState()
        {
            foreach (var dynamicObject in ObjectsCollection.OfType<IDynamicObject>())
            {
                dynamicObject.Updated = false;
            }
        }

        private void ProcessDynamicObjects(Point position, UpdateOrder updateOrder)
        {
            var dynamicObjects = ObjectsCollection.OfType<IDynamicObject>()
                .Where(obj => !obj.Updated && obj.UpdateOrder == updateOrder).ToArray();
            foreach (var dynamicObject in dynamicObjects)
            {
                dynamicObject.Update(position);
                dynamicObject.Updated = true;
            }
        }

        private void ProcessDestroyableObjects(IAreaMap map, Point position)
        {
            var destroyableObjects = ObjectsCollection.OfType<IDestroyableObject>().ToArray();
            ProcessStatusesAndEnvironment(destroyableObjects, position);

            var deadObjects = destroyableObjects.Where(obj => obj.Health <= 0).ToArray();
            foreach (var deadObject in deadObjects)
            {
                map.RemoveObject(position, deadObject);
                deadObject.OnDeath(position);
            }
        }

        private void ProcessStatusesAndEnvironment(IDestroyableObject[] destroyableObjects, Point position)
        {
            foreach (var destroyableObject in destroyableObjects)
            {
                destroyableObject.Statuses.Update(position);
            }
        }

        public void CheckSpreading(IAreaMapCellInternal other)
        {
            CheckSpreadingObjects(other);
        }

        private void CheckSpreadingObjects(IAreaMapCellInternal other)
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
                    SpreadObject(otherSpreadingObject, this);
                }
            }
        }

        private void SpreadObject(ISpreadingObject liquid, IAreaMapCellInternal target)
        {
            var spreadAmount = Math.Min(liquid.MaxSpreadVolume, liquid.Volume - liquid.MaxVolumeBeforeSpread);
            var separated = liquid.Separate(spreadAmount);
            target.ObjectsCollection.AddVolumeObject(separated);
        }
    }
}