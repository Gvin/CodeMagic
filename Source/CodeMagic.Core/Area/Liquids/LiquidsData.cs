using System;
using System.Collections.Generic;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area.Liquids
{
    public class LiquidsData
    {
        private readonly Dictionary<Type, ILiquid> liquids;

        public LiquidsData()
        {
            liquids = new Dictionary<Type, ILiquid>();
        }

        public void AddLiquid(ILiquid liquid)
        {
            var type = liquid.GetType();
            if (liquids.ContainsKey(type))
            {
                liquids[type].Volume += liquid.Volume;
                return;
            }

            liquids.Add(type, liquid);
        }

        public int GetLiquidVolume<T>() where T : ILiquid
        {
            if (!liquids.ContainsKey(typeof(T)))
                return 0;
            return liquids[typeof(T)].Volume;
        }

        public bool ContainsLiquid<T>(int volume = 1) where T : ILiquid
        {
            var existingVolume = GetLiquidVolume<T>();
            return existingVolume >= volume;
        }

        public void RemoveLiquid<T>(int volume) where T : ILiquid
        {
            if (!liquids.ContainsKey(typeof(T)))
                throw new ArgumentException($"Liquids storage doesn't contain {volume} units of liquid \"{typeof(T).FullName}\".");
            liquids[typeof(T)].Volume -= volume;
        }

        public void CheckLiquidSpreading(LiquidsData other)
        {
            foreach (var liquid in liquids.Values)
            {
                if (liquid.Volume >= liquid.MaxVolume)
                {
                    SpreadLiquid(liquid, other);
                }
            }

            foreach (var otherLiquid in other.liquids.Values)
            {
                if (otherLiquid.Volume >= otherLiquid.MaxVolume)
                {
                    SpreadLiquid(otherLiquid, other);
                }
            }
        }

        private void SpreadLiquid(ILiquid liquid, LiquidsData target)
        {
            var spreadAmount = Math.Min(liquid.MaxSpreadVolume, liquid.Volume - liquid.MaxVolume);
            var separated = liquid.Separate(spreadAmount);
            target.AddLiquid(separated);
        }

        public void ApplyLiquids(IDestroyableObject destroyable, Journal journal)
        {
            foreach (var liquid in liquids.Values)
            {
                liquid.ApplyEffect(destroyable, journal);
            }
        }

        public void UpdateLiquids(AreaMapCell cell)
        {
            foreach (var liquid in liquids.Values)
            {
                liquid.Update(cell);
            }
        }
    }
}