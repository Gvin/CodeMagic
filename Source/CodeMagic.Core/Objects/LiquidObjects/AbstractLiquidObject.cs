using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Objects.SteamObjects;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public abstract class AbstractLiquidObject : ILiquidObject, IDynamicObject
    {
        protected readonly ILiquidConfiguration Configuration;
        private int volume;

        protected AbstractLiquidObject(int volume, string type)
        {
            Configuration = ConfigurationManager.GetLiquidConfiguration(type);
            Type = type;
            this.volume = volume;
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public abstract string Name { get; }

        public string Type { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

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
            var volumeToLowerTemp = (int)Math.Floor(excessTemperature * Configuration.EvaporationTemperatureMultiplier);
            var volumeToBecomeSteam = Math.Min(volumeToLowerTemp, Volume);
            var heatLoss = (int)Math.Floor(volumeToBecomeSteam * Configuration.EvaporationTemperatureMultiplier);

            cell.Environment.Temperature -= heatLoss;
            Volume -= volumeToBecomeSteam;

            var steamVolume = volumeToBecomeSteam * Configuration.EvaporationMultiplier;
            cell.Environment.Pressure += steamVolume * Configuration.Steam.PressureMultiplier;
            
            cell.Objects.AddVolumeObject(CreateSteam(steamVolume));
        }

        protected abstract ISteamObject CreateSteam(int volume);

        private void ProcessFreezing(AreaMapCell cell)
        {
            var missingTemperature = Configuration.FreezingPoint - cell.Environment.Temperature;
            var volumeToRaiseTemp = (int)Math.Floor(missingTemperature * Configuration.FreezingTemperatureMultiplier);
            var volumeToFreeze = Math.Min(volumeToRaiseTemp, Volume);
            var heatGain = (int)Math.Floor(volumeToFreeze / Configuration.FreezingTemperatureMultiplier);

            cell.Environment.Temperature += heatGain;
            Volume -= volumeToFreeze;

            cell.Objects.AddVolumeObject(CreateIce(volumeToFreeze));
        }

        protected abstract IIceObject CreateIce(int volume);

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

        public abstract ISpreadingObject Separate(int volume);

        protected string GetCustomConfigurationValue(string key)
        {
            var stringValue = Configuration.CustomValues
                .FirstOrDefault(value => string.Equals(value.Key, key))?.Value;
            if (string.IsNullOrEmpty(stringValue))
                throw new ApplicationException($"Custom value {key} not found in the configuration for \"{Type}\".");

            return stringValue;
        }

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }
    }
}