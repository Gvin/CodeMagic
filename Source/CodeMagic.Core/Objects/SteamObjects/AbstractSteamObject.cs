using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.SteamObjects
{
    public interface ISteamObject : ISpreadingObject
    {
    }

    public abstract class AbstractSteamObject : ISteamObject, IDynamicObject
    {
        protected readonly ILiquidConfiguration Configuration;
        private int volume;

        protected AbstractSteamObject(int volume, string liquidType)
        {
            Configuration = ConfigurationManager.GetLiquidConfiguration(liquidType);
            if (Configuration == null)
                throw new ApplicationException($"Unable to find liquid configuration for liquid type \"{liquidType}\".");

            this.volume = volume;
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public abstract string Name { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => Thickness >= 100;

        public bool BlocksEnvironment => false;

        protected int Thickness => (int)(Volume * Configuration.Steam.ThicknessMultiplier);

        public ZIndex ZIndex => ZIndex.Air;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public abstract string Type { get; }

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

        public int MaxVolumeBeforeSpread => Configuration.Steam.MaxVolumeBeforeSpread;

        public int MaxSpreadVolume => Configuration.Steam.MaxSpreadVolume;

        public abstract ISpreadingObject Separate(int volume);

        protected string GetCustomConfigurationValue(string key)
        {
            var stringValue = Configuration.CustomValues
                .FirstOrDefault(value => string.Equals(value.Key, key))?.Value;
            if (string.IsNullOrEmpty(stringValue))
                throw new ApplicationException($"Custom value {key} not found in the configuration for \"{Type}\".");

            return stringValue;
        }

        public void Update(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            if (Volume == 0)
            {
                cell.Objects.Remove(this);
                return;
            }

            if (cell.Environment.Temperature < Configuration.BoilingPoint)
            {
                ProcessCondensation(cell);
            }

            UpdateSteam(game, position);
        }

        protected virtual void UpdateSteam(IGameCore game, Point position)
        {
            // Do nothing.
        }

        private void ProcessCondensation(AreaMapCell cell)
        {
            var missingTemperature = Configuration.BoilingPoint - cell.Environment.Temperature;
            var volumeToRaiseTemp = (int)Math.Floor(missingTemperature * Configuration.CondensationTemperatureMultiplier);
            var volumeToCondense = Math.Min(volumeToRaiseTemp, Volume);
            var heatGain = (int)Math.Floor(volumeToCondense / Configuration.CondensationTemperatureMultiplier);

            cell.Environment.Temperature += heatGain;
            Volume -= volumeToCondense;

            var liquidVolume = volumeToCondense / Configuration.EvaporationMultiplier;
            cell.Objects.AddVolumeObject(CreateLiquid(liquidVolume));
        }

        protected abstract ILiquidObject CreateLiquid(int volume);

        public bool Updated { get; set; }
    }
}