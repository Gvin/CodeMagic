using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public class OilLiquidObject : ILiquidObject
    {
        private const string CustomValueIgnitionTemperature = "IgnitionTemperature";
        private const string CustomValueBurningTemperature = "BurningTemperature";
        private const string CustomValueBurningRate = "BurningRate";
        private const string CustomValueHeatSpeed = "HeatSpeed";

        public const string LiquidType = "oil";

        protected readonly ILiquidConfiguration Configuration;

        private readonly int ignitionTemperature;
        private readonly int burningTemperature;
        private readonly int heatSpeed;
        private readonly double burningRate;

        private int volume;

        public OilLiquidObject(int volume)
        {
            Configuration = ConfigurationManager.GetLiquidConfiguration(LiquidType);

            ignitionTemperature = GetCustomInt(CustomValueIgnitionTemperature);
            burningTemperature = GetCustomInt(CustomValueBurningTemperature);
            heatSpeed = GetCustomInt(CustomValueHeatSpeed);
            burningRate = GetCustomDouble(CustomValueBurningRate);

            this.volume = volume;
        }

        public ZIndex ZIndex => ZIndex.FloorCover;

        public string Name => "Oil";

        public void Update(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);

            if (Volume <= 0)
            {
                cell.Objects.Remove(this);
                return;
            }

            if (cell.Environment.Temperature >= ignitionTemperature)
            {
                ProcessBurning(cell);
            }

            if (Volume >= MinVolumeForEffect)
            { 
                ApplyOilyStatus(cell);
            }
        }

        private void ApplyOilyStatus(AreaMapCell cell)
        {
            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                destroyable.Statuses.Add(new OilyObjectStatus(Configuration));
            }
        }

        private void ProcessBurning(AreaMapCell cell)
        {
            if (cell.Environment.Temperature < burningTemperature)
            {
                var temperatureDiff = burningTemperature - cell.Environment.Temperature;
                var temperatureChange = Math.Min(temperatureDiff, heatSpeed);
                cell.Environment.Temperature += temperatureChange;
            }

            var burnedVolume = (int)Math.Ceiling(cell.Environment.Temperature * burningRate);
            Volume -= burnedVolume;
        }

        private int GetCustomInt(string key)
        {
            var stringValue = GetCustomString(key);
            return int.Parse(stringValue);
        }

        private string GetCustomString(string key)
        {
            var stringValue =
                Configuration.CustomValues.FirstOrDefault(value =>
                    string.Equals(value.Key, key))?.Value;
            if (string.IsNullOrEmpty(stringValue))
                throw new ArgumentException(
                    $"Custom value \"{key}\" not found in configuration for liquid type \"{LiquidType}\".");
            return stringValue;
        }

        private double GetCustomDouble(string key)
        {
            var stringValue = GetCustomString(key);
            return double.Parse(stringValue);
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

        public int MaxVolumeBeforeSpread => Configuration.MaxVolumeBeforeSpread;

        public int MaxSpreadVolume => Configuration.MaxSpreadVolume;

        public int MinVolumeForEffect => Configuration.MinVolumeForEffect;

        public ILiquidObject Separate(int separateVolume)
        {
            Volume -= separateVolume;
            return new OilLiquidObject(separateVolume);
        }

        public bool Updated { get; set; }

        #region IMapObject Implementation

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => Volume >= MinVolumeForEffect;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        #endregion
    }
}