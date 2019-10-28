using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.IceObjects;
using CodeMagic.Game.Objects.SteamObjects;

namespace CodeMagic.Game.Objects.LiquidObjects
{
    public abstract class AbstractLiquid : ILiquid, IDynamicObject
    {
        protected readonly ILiquidConfiguration Configuration;
        private int volume;

        protected AbstractLiquid(int volume, string type)
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

        public bool BlocksAttack => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.FloorCover;

        public ObjectSize Size => ObjectSize.Huge;

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            var cell = map.GetCell(position);

            if (Volume == 0)
            {
                map.RemoveObject(position, this);
                return;
            }

            if (cell.Temperature >= Configuration.BoilingPoint)
            {
                ProcessBoiling(map, position, cell);
            }

            if (cell.Temperature <= Configuration.FreezingPoint)
            {
                ProcessFreezing(map, position, cell);
            }

            UpdateLiquid(map, journal, position);
        }

        protected virtual void UpdateLiquid(IAreaMap map, IJournal journal, Point position)
        {
            // Do nothing.
        }

        private void ProcessBoiling(IAreaMap map, Point position, IAreaMapCell cell)
        {
            var excessTemperature = cell.Temperature - Configuration.BoilingPoint;
            var volumeToLowerTemp = (int)Math.Floor(excessTemperature * Configuration.EvaporationTemperatureMultiplier);
            var volumeToBecomeSteam = Math.Min(volumeToLowerTemp, Volume);
            var heatLoss = (int)Math.Floor(volumeToBecomeSteam * Configuration.EvaporationTemperatureMultiplier);

            cell.Temperature -= heatLoss;
            Volume -= volumeToBecomeSteam;

            var steamVolume = volumeToBecomeSteam * Configuration.EvaporationMultiplier;
            cell.Pressure += steamVolume * Configuration.Steam.PressureMultiplier;
            
            map.AddObject(position, CreateSteam(steamVolume));
        }

        protected abstract ISteam CreateSteam(int volume);

        private void ProcessFreezing(IAreaMap map, Point position, IAreaMapCell cell)
        {
            var missingTemperature = Configuration.FreezingPoint - cell.Temperature;
            var volumeToRaiseTemp = (int)Math.Floor(missingTemperature * Configuration.FreezingTemperatureMultiplier);
            var volumeToFreeze = Math.Min(volumeToRaiseTemp, Volume);
            var heatGain = (int)Math.Floor(volumeToFreeze / Configuration.FreezingTemperatureMultiplier);

            cell.Temperature += heatGain;
            Volume -= volumeToFreeze;

            map.AddObject(position, CreateIce(volumeToFreeze));
        }

        protected abstract IIce CreateIce(int volume);

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