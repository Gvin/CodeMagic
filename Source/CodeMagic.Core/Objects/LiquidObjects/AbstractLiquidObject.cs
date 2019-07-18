using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.IceObjects;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public abstract class AbstractLiquidObject<TIce> : ILiquidObject where TIce : IIceObject
    {
        protected readonly ILiquidConfiguration Configuration;
        private readonly string type;
        private int volume;

        protected AbstractLiquidObject(int volume, string type)
        {
            Configuration = ConfigurationManager.GetLiquidConfiguration(type);
            this.type = type;
            this.volume = volume;
        }

        public abstract string Name { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => Volume >= MinVolumeForEffect;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.FloorCover;

        public void Update(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);

            if (Volume == 0)
            {
                cell.Objects.Remove(this);
                return;
            }

            if (cell.Environment.Temperature >= Configuration.BoilingPoint)
            {
                ProcessBoiling(cell);
            }

            if (cell.Environment.Temperature <= Configuration.FreezingPoint)
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
            var excessTemperature = cell.Environment.Temperature - Configuration.BoilingPoint;
            var steamVolume = Math.Min(excessTemperature * Configuration.EvaporationMultiplier, Volume);
            var heatLoss = steamVolume / Configuration.EvaporationMultiplier;

            cell.Environment.Temperature -= heatLoss;
            cell.Environment.Pressure += steamVolume * Configuration.SteamPressureMultiplier;
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

        public int MaxVolumeBeforeSpread => Configuration.MaxVolumeBeforeSpread;

        public int MinVolumeForEffect => Configuration.MinVolumeForEffect;

        public int MaxSpreadVolume => Configuration.MaxSpreadVolume;

        public abstract ILiquidObject Separate(int volume);

        protected string GetCustomConfigurationValue(string key)
        {
            var stringValue = Configuration.CustomValues
                .FirstOrDefault(value => string.Equals(value.Key, key))?.Value;
            if (string.IsNullOrEmpty(stringValue))
                throw new ApplicationException($"Custom value {key} not found in the configuration for \"{type}\".");

            return stringValue;
        }
    }
}