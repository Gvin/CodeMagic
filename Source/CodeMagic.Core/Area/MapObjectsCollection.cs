using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public class MapObjectsCollection : IEnumerable<IMapObject>
    {
        private readonly ConcurrentList<IMapObject> objects;

        public MapObjectsCollection()
        {
            objects = new ConcurrentList<IMapObject>();
        }

        public MapObjectsCollection(IEnumerable<IMapObject> objects)
        {
            this.objects = new ConcurrentList<IMapObject>();
            foreach (var mapObject in objects)
            {
                this.objects.Add(mapObject);
            }
        }

        public void Add(IMapObject mapObject)
        {
            switch (mapObject)
            {
                case IVolumeObject volumeObject:
                    AddVolumeObject(volumeObject);
                    break;
                default:
                    objects.Add(mapObject);
                    break;
            }
        }

        public void AddVolumeObject(IVolumeObject volumeObject)
        {
            var oldObject = objects.OfType<IVolumeObject>()
                .FirstOrDefault(volObj => string.Equals(volObj.Type, volumeObject.Type));
            if (oldObject == null)
            {
                objects.Add(volumeObject);
                return;
            }

            oldObject.Volume += volumeObject.Volume;
        }

        public int GetVolume<TObject>() where TObject : IVolumeObject
        {
            var obj = objects.OfType<TObject>().FirstOrDefault();
            return obj?.Volume ?? 0;
        }

        public void RemoveVolume<TObject>(int volume) where TObject : IVolumeObject
        {
            var obj = objects.OfType<TObject>().FirstOrDefault();
            if (obj == null)
                throw new ArgumentException($"Volume object {typeof(TObject).FullName} not found in cell.");
            if (obj.Volume < volume)
                throw new ArgumentException($"Not enough volume of {typeof(TObject).FullName} in the cell. Removing {volume} but only have {obj.Volume}");

            obj.Volume -= volume;
            if (obj.Volume == 0)
            {
                objects.Remove(obj);
            }
        }

        public void Remove(IMapObject mapObject)
        {
            objects.Remove(mapObject);
        }

        public IEnumerator<IMapObject> GetEnumerator()
        {
            return objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}