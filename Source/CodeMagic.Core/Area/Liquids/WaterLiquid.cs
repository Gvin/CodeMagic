using System;
using System.Linq;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Area.Liquids
{
    public class WaterLiquid : AbstractLiquid
    {
        public const int WaterFreezingPoint = 0;
        private const int WaterBoilingPoint = 100;

        private const int WaterMaxVolume = 100;
        private const int WaterMaxSpreadVolume = 3;

        private const int WaterRequiredPerDegrees = 1;
        private const int SteamToPressureMultiplier = 6;

        public const int MinVolumeForEffect = 50;
        
        public WaterLiquid(int volume)
            : base(volume)
        {
        }

        public override int MaxVolume => WaterMaxVolume;

        public override int MaxSpreadVolume => WaterMaxSpreadVolume;

        protected override int FreezingPoint => WaterFreezingPoint;

        protected override int BoilingPoint => WaterBoilingPoint;

        public override ILiquid Separate(int separateVolume)
        {
            Volume -= separateVolume;
            return new WaterLiquid(separateVolume);
        }

        protected override void ProcessBoiling(AreaMapCell cell)
        {
            var excessTemperature = cell.Environment.Temperature - WaterBoilingPoint;
            var steamVolume = Math.Min(excessTemperature * WaterRequiredPerDegrees, Volume);
            var heatLoss = steamVolume / WaterRequiredPerDegrees;

            cell.Environment.Temperature -= heatLoss;
            cell.Environment.Pressure += steamVolume * SteamToPressureMultiplier;
            Volume -= steamVolume;
        }

        protected override void ProcessFreezing(AreaMapCell cell)
        {
            var ice = cell.Objects.OfType<IceObject>().FirstOrDefault();
            if (ice == null)
            {
                ice = new IceObject(0);
                cell.Objects.Add(ice);
            }

            ice.Volume += Volume;
            Volume = 0;
        }

        public override void ApplyEffect(IDestroyableObject destroyable, Journal journal)
        {
            if (Volume < MinVolumeForEffect)
                return;

            if (destroyable.Statuses.Contains(OnFireObjectStatus.StatusType))
            {
                destroyable.Statuses.Remove(OnFireObjectStatus.StatusType);
            }

            destroyable.Statuses.Add(new WetObjectStatus());
        }
    }
}