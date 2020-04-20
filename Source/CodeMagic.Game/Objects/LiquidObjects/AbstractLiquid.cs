using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Objects.IceObjects;
using CodeMagic.Game.Objects.SteamObjects;

namespace CodeMagic.Game.Objects.LiquidObjects
{
    public abstract class AbstractLiquid : MapObjectBase, ILiquid, IDynamicObject
    {
        private const string SaveKeyVolume = "Volume";
        private const string SaveKeyLiquidType = "LiquidType";

        protected readonly ILiquidConfiguration Configuration;
        private int volume;

        protected AbstractLiquid(SaveData data) : base(data)
        {
            Type = data.GetStringValue(SaveKeyLiquidType);
            Configuration = ConfigurationManager.GetLiquidConfiguration(Type);
            volume = data.GetIntValue(SaveKeyVolume);
        }

        protected AbstractLiquid(int volume, string type, string name)
            : base(name)
        {
            Configuration = ConfigurationManager.GetLiquidConfiguration(type);
            Type = type;
            this.volume = volume;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyVolume, volume);
            data.Add(SaveKeyLiquidType, Type);
            return data;
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public string Type { get; }

        public override ZIndex ZIndex => ZIndex.FloorCover;

        public override ObjectSize Size => ObjectSize.Huge;

        public void Update(Point position)
        {
            var cell = CurrentGame.Map.GetCell(position);

            if (Volume == 0)
            {
                CurrentGame.Map.RemoveObject(position, this);
                return;
            }

            if (cell.Temperature() >= Configuration.BoilingPoint)
            {
                ProcessBoiling(position, cell);
            }

            if (cell.Temperature() <= Configuration.FreezingPoint)
            {
                ProcessFreezing(position, cell);
            }

            UpdateLiquid(position);
        }

        protected virtual void UpdateLiquid(Point position)
        {
            // Do nothing.
        }

        private void ProcessBoiling(Point position, IAreaMapCell cell)
        {
            var excessTemperature = cell.Temperature() - Configuration.BoilingPoint;
            var volumeToLowerTemp = (int)Math.Floor(excessTemperature * Configuration.EvaporationTemperatureMultiplier);
            var volumeToBecomeSteam = Math.Min(volumeToLowerTemp, Volume);
            var heatLoss = (int)Math.Floor(volumeToBecomeSteam * Configuration.EvaporationTemperatureMultiplier);

            cell.Environment.Cast().Temperature -= heatLoss;
            Volume -= volumeToBecomeSteam;

            var steamVolume = volumeToBecomeSteam * Configuration.EvaporationMultiplier;
            cell.Environment.Cast().Pressure += steamVolume * Configuration.Steam.PressureMultiplier;
            
            CurrentGame.Map.AddObject(position, CreateSteam(steamVolume));
        }

        protected abstract ISteam CreateSteam(int volume);

        private void ProcessFreezing(Point position, IAreaMapCell cell)
        {
            var missingTemperature = Configuration.FreezingPoint - cell.Temperature();
            var volumeToRaiseTemp = (int)Math.Floor(missingTemperature * Configuration.FreezingTemperatureMultiplier);
            var volumeToFreeze = Math.Min(volumeToRaiseTemp, Volume);
            var heatGain = (int)Math.Floor(volumeToFreeze / Configuration.FreezingTemperatureMultiplier);

            cell.Environment.Cast().Temperature += heatGain;
            Volume -= volumeToFreeze;

            CurrentGame.Map.AddObject(position, CreateIce(volumeToFreeze));
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
    }
}