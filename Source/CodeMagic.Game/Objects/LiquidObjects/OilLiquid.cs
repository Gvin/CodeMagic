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
using CodeMagic.Game.Statuses;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.LiquidObjects
{
    public class OilLiquid : MapObjectBase, ILiquid, IFireSpreadingObject, IDynamicObject, IWorldImageProvider
    {
        private const string SaveKeyVolume = "Volume";

        private const string ImageSmall = "Oil_Small";
        private const string ImageMedium = "Oil_Medium";
        private const string ImageBig = "Oil_Big";

        private const string CustomValueIgnitionTemperature = "IgnitionTemperature";
        private const string CustomValueBurningTemperature = "BurningTemperature";
        private const string CustomValueBurningRate = "BurningRate";
        private const string CustomValueHeatSpeed = "HeatSpeed";

        public const string LiquidType = "OilLiquid";

        private readonly ILiquidConfiguration configuration;

        private readonly int ignitionTemperature;
        private readonly int heatSpeed;
        private readonly double burningRate;

        private int volume;

        public OilLiquid(SaveData data) : base(data)
        {
            configuration = ConfigurationManager.GetLiquidConfiguration(LiquidType);

            ignitionTemperature = GetCustomInt(CustomValueIgnitionTemperature);
            BurningTemperature = GetCustomInt(CustomValueBurningTemperature);
            heatSpeed = GetCustomInt(CustomValueHeatSpeed);
            burningRate = GetCustomDouble(CustomValueBurningRate);

            volume = data.GetIntValue(SaveKeyVolume);
        }

        public OilLiquid(int volume)
            : base("Oil")
        {
            configuration = ConfigurationManager.GetLiquidConfiguration(LiquidType);

            ignitionTemperature = GetCustomInt(CustomValueIgnitionTemperature);
            BurningTemperature = GetCustomInt(CustomValueBurningTemperature);
            heatSpeed = GetCustomInt(CustomValueHeatSpeed);
            burningRate = GetCustomDouble(CustomValueBurningRate);

            this.volume = volume;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyVolume, volume);
            return data;
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public override ZIndex ZIndex => ZIndex.FloorCover;

        public override ObjectSize Size => ObjectSize.Huge;

        public string Type => LiquidType;

        public void Update(Point position)
        {
            var cell = CurrentGame.Map.GetCell(position);

            if (Volume <= 0)
            {
                CurrentGame.Map.RemoveObject(position, this);
                return;
            }

            if (cell.Temperature() >= ignitionTemperature)
            {
                ProcessBurning(cell);
            }

            if (Volume >= MinVolumeForEffect)
            {
                ApplyOilyStatus(cell);
            }
        }

        private void ApplyOilyStatus(IAreaMapCell cell)
        {
            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                destroyable.Statuses.Add(new OilyObjectStatus());
            }
        }

        private void ProcessBurning(IAreaMapCell cell)
        {
            if (cell.Temperature() < BurningTemperature)
            {
                var temperatureDiff = BurningTemperature - cell.Temperature();
                var temperatureChange = Math.Min(temperatureDiff, heatSpeed);
                cell.Environment.Cast().Temperature += temperatureChange;
            }

            var burnedVolume = (int)Math.Ceiling(cell.Temperature() * burningRate);
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
                configuration.CustomValues.FirstOrDefault(value =>
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
            set => volume = Math.Max(0, value);
        }

        public int MaxVolumeBeforeSpread => configuration.MaxVolumeBeforeSpread;

        public int MaxSpreadVolume => configuration.MaxSpreadVolume;

        public int MinVolumeForEffect => configuration.MinVolumeForEffect;

        public ISpreadingObject Separate(int separateVolume)
        {
            Volume -= separateVolume;
            return new OilLiquid(separateVolume);
        }

        public bool Updated { get; set; }

        public bool GetIsOnFire(IAreaMapCell cell)
        {
            return cell.Temperature() >= ignitionTemperature;
        }

        public bool SpreadsFire => true;

        public int BurningTemperature { get; }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (Volume >= configuration.MaxVolumeBeforeSpread)
                return storage.GetImage(ImageBig);

            var halfSpread = configuration.MaxVolumeBeforeSpread / 2;
            if (Volume >= halfSpread)
                return storage.GetImage(ImageMedium);

            return storage.GetImage(ImageSmall);
        }
    }
}