using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Area
{
    public class MapObjectsCollection : IEnumerable<IMapObject>
    {
        private readonly List<IMapObject> objects;

        public MapObjectsCollection()
        {
            objects = new List<IMapObject>();
        }

        public void Add(IMapObject mapObject)
        {
            switch (mapObject)
            {
                case ILiquidObject liquid:
                    AddLiquid(liquid);
                    break;
                case IIceObject ice:
                    AddIce(ice);
                    break;
                default:
                    objects.Add(mapObject);
                    break;
            }
        }

        public void AddLiquid(ILiquidObject liquid)
        {
            var oldLiquid = objects.FirstOrDefault(mapObject => mapObject.GetType() == liquid.GetType()) as ILiquidObject;
            if (oldLiquid == null)
            {
                objects.Add(liquid);
                return;
            }

            oldLiquid.Volume += liquid.Volume;
        }

        public void AddIce(IIceObject ice)
        {
            var oldIce = objects.FirstOrDefault(mapObject => mapObject.GetType() == ice.GetType()) as IIceObject;
            if (oldIce == null)
            {
                objects.Add(ice);
                return;
            }

            oldIce.Volume += ice.Volume;
        }

        public int GetLiquidVolume<TLiquid>() where TLiquid : ILiquidObject
        {
            var liquid = objects.OfType<TLiquid>().FirstOrDefault();
            return liquid?.Volume ?? 0;
        }

        public void RemoveLiquidVolume<TLiquid>(int volume) where TLiquid : ILiquidObject
        {
            var liquid = objects.OfType<TLiquid>().FirstOrDefault();
            if (liquid == null)
                throw new ArgumentException($"Liquid {typeof(TLiquid).FullName} not found in cell.");
            if (liquid.Volume < volume)
                throw new ArgumentException($"Not enough liquid {typeof(TLiquid).FullName} in the cell. Removing {volume} but only have {liquid.Volume}");

            liquid.Volume -= volume;
            if (liquid.Volume == 0)
            {
                objects.Remove(liquid);
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