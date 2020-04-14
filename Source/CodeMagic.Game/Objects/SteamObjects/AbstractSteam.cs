using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Objects.SteamObjects
{
    public interface ISteam : ISpreadingObject
    {
    }

    public abstract class AbstractSteam : ISteam, IDynamicObject
    {
        protected readonly ILiquidConfiguration Configuration;
        private int volume;

        protected AbstractSteam(int volume, string liquidType)
        {
            Configuration = ConfigurationManager.GetLiquidConfiguration(liquidType);
            if (Configuration == null)
                throw new ApplicationException($"Unable to find liquid configuration for liquid type \"{liquidType}\".");

            this.volume = volume;
        }

        public ObjectSize Size => ObjectSize.Huge;

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public abstract string Name { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool BlocksAttack => false;

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

        public void Update(Point position)
        {
            var cell = CurrentGame.Map.GetCell(position);
            if (Volume == 0)
            {
                CurrentGame.Map.RemoveObject(position, this);
                return;
            }

            if (cell.Temperature() < Configuration.BoilingPoint)
            {
                ProcessCondensation(position, cell);
            }

            UpdateSteam(position);
        }

        protected virtual void UpdateSteam(Point position)
        {
            // Do nothing.
        }

        private void ProcessCondensation(Point position, IAreaMapCell cell)
        {
            var missingTemperature = Configuration.BoilingPoint - cell.Temperature();
            var volumeToRaiseTemp = (int)Math.Floor(missingTemperature * Configuration.CondensationTemperatureMultiplier);
            var volumeToCondense = Math.Min(volumeToRaiseTemp, Volume);
            var heatGain = (int)Math.Floor(volumeToCondense / Configuration.CondensationTemperatureMultiplier);

            cell.Environment.Cast().Temperature += heatGain;
            Volume -= volumeToCondense;

            var liquidVolume = volumeToCondense / Configuration.EvaporationMultiplier;

            CurrentGame.Map.AddObject(position, CreateLiquid(liquidVolume));
        }

        protected abstract ILiquid CreateLiquid(int volume);

        public bool Updated { get; set; }
    }
}