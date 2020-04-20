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
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Objects.SteamObjects
{
    public interface ISteam : ISpreadingObject
    {
    }

    public abstract class AbstractSteam : MapObjectBase, ISteam, IDynamicObject
    {
        private const string SaveKeyVolume = "Volume";
        private const string SaveKeyLiquidType = "LiquidType";

        private readonly ILiquidConfiguration configuration;
        private readonly string liquidType;
        private int volume;

        protected AbstractSteam(SaveData data) : base(data)
        {
            liquidType = data.GetStringValue(SaveKeyLiquidType);
            configuration = ConfigurationManager.GetLiquidConfiguration(liquidType);
            if (configuration == null)
                throw new ApplicationException($"Unable to find liquid configuration for liquid type \"{liquidType}\".");

            volume = data.GetIntValue(SaveKeyVolume);
        }

        protected AbstractSteam(int volume, string liquidType, string name)
            : base(name)
        {
            this.liquidType = liquidType;
            configuration = ConfigurationManager.GetLiquidConfiguration(liquidType);
            if (configuration == null)
                throw new ApplicationException($"Unable to find liquid configuration for liquid type \"{liquidType}\".");

            this.volume = volume;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyVolume, volume);
            data.Add(SaveKeyLiquidType, liquidType);
            return data;
        }

        public override ObjectSize Size => ObjectSize.Huge;

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public override bool BlocksVisibility => Thickness >= 100;

        protected int Thickness => (int)(Volume * configuration.Steam.ThicknessMultiplier);

        public override ZIndex ZIndex => ZIndex.Air;

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

        public int MaxVolumeBeforeSpread => configuration.Steam.MaxVolumeBeforeSpread;

        public int MaxSpreadVolume => configuration.Steam.MaxSpreadVolume;

        public abstract ISpreadingObject Separate(int volume);

        protected string GetCustomConfigurationValue(string key)
        {
            var stringValue = configuration.CustomValues
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

            if (cell.Temperature() < configuration.BoilingPoint)
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
            var missingTemperature = configuration.BoilingPoint - cell.Temperature();
            var volumeToRaiseTemp = (int)Math.Floor(missingTemperature * configuration.CondensationTemperatureMultiplier);
            var volumeToCondense = Math.Min(volumeToRaiseTemp, Volume);
            var heatGain = (int)Math.Floor(volumeToCondense / configuration.CondensationTemperatureMultiplier);

            cell.Environment.Cast().Temperature += heatGain;
            Volume -= volumeToCondense;

            var liquidVolume = volumeToCondense / configuration.EvaporationMultiplier;

            CurrentGame.Map.AddObject(position, CreateLiquid(liquidVolume));
        }

        protected abstract ILiquid CreateLiquid(int volume);

        public bool Updated { get; set; }
    }
}