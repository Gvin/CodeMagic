using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.IceObjects;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public abstract class AbstractLiquidObject<TIce> : ILiquidObject where TIce : IIceObject
    {
        private int volume;

        protected AbstractLiquidObject(int volume)
        {
            this.volume = volume;
        }

        public abstract string Name { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => Volume >= MinVolumeForEffect;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        protected abstract int FreezingPoint { get; }

        protected abstract int BoilingPoint { get; }

        protected abstract int MinVolumeForEffect { get; }

        protected abstract int LiquidConsumptionPerTemperature { get; }

        protected abstract int SteamToPressureMultiplier { get; }

        public void Update(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);

            if (Volume == 0)
            {
                cell.Objects.Remove(this);
                return;
            }

            if (cell.Environment.Temperature >= BoilingPoint)
            {
                ProcessBoiling(cell);
            }

            if (cell.Environment.Temperature <= FreezingPoint)
            {
                ProcessFreezing(cell);
            }

            UpdateLiquid(game, position);
        }

        protected virtual void UpdateLiquid(IGameCore game, Point position)
        {
            // Do nothing.
        }

        private void ProcessBoiling(AreaMapCell cell)
        {
            var excessTemperature = cell.Environment.Temperature - BoilingPoint;
            var steamVolume = Math.Min(excessTemperature * LiquidConsumptionPerTemperature, Volume);
            var heatLoss = steamVolume / LiquidConsumptionPerTemperature;

            cell.Environment.Temperature -= heatLoss;
            cell.Environment.Pressure += steamVolume * SteamToPressureMultiplier;
            Volume -= steamVolume;
        }

        private void ProcessFreezing(AreaMapCell cell)
        {
            var ice = cell.Objects.OfType<TIce>().FirstOrDefault();
            if (ice == null)
            {
                ice = CreateIce(0);
                cell.Objects.AddIce(ice);
            }

            ice.Volume += Volume;
            Volume = 0;
        }

        protected abstract TIce CreateIce(int volume);

        public bool Updated { get; set; }

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

        public abstract int MaxVolumeBeforeSpread { get; }

        public abstract int MaxSpreadVolume { get; }

        public abstract ILiquidObject Separate(int volume);
    }
}