using System;
using System.Linq;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Area.Liquids
{
    public class WaterLiquid : ILiquid
    {
        private const int WaterMaxVolume = 100;
        private const int WaterMaxSpreadVolume = 3;

        private const int BoilingTemperature = 100;
        private const int WaterRequiredPerDegrees = 1;
        private const int SteamToPressureMultiplier = 6;

        public const int MinVolumeForEffect = 50;
        public const int FreezingPoint = 0;

        private int volume;

        public WaterLiquid(int volume)
        {
            this.volume = volume;
        }

        public int Volume
        {
            get => volume;
            set
            {
                if (value < 0)
                {
                    volume = 0;
                    return;
                }

                volume = value;
            }
        }

        public int MaxVolume => WaterMaxVolume;

        public int MaxSpreadVolume => WaterMaxSpreadVolume;

        public ILiquid Separate(int separateVolume)
        {
            Volume -= separateVolume;
            return new WaterLiquid(separateVolume);
        }

        public void Update(AreaMapCell cell)
        {
            if (Volume == 0)
                return;

            if (cell.Environment.Temperature <= FreezingPoint)
            {
                ProcessFreezing(cell);
                return;
            }

            if (cell.Environment.Temperature >= BoilingTemperature)
            {
                ProcessBoiling(cell);
            }
        }

        private void ProcessBoiling(AreaMapCell cell)
        {
            var excessTemperature = cell.Environment.Temperature - BoilingTemperature;
            var steamVolume = Math.Min(excessTemperature * WaterRequiredPerDegrees, Volume);
            var heatLoss = steamVolume / WaterRequiredPerDegrees;

            cell.Environment.Temperature -= heatLoss;
            cell.Environment.Pressure += steamVolume * SteamToPressureMultiplier;
            Volume -= steamVolume;
        }

        private void ProcessFreezing(AreaMapCell cell)
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

        public void ApplyEffect(IDestroyableObject destroyable)
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