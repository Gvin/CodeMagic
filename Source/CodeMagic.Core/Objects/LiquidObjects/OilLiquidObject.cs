using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public interface IOilLiquidObject : ILiquidObject, IInjectable
    {
    }

    public class OilLiquidObject : IOilLiquidObject, IFireSpreadingObject, IDynamicObject
    {
        private const string CustomValueIgnitionTemperature = "IgnitionTemperature";
        private const string CustomValueBurningTemperature = "BurningTemperature";
        private const string CustomValueBurningRate = "BurningRate";
        private const string CustomValueHeatSpeed = "HeatSpeed";

        public const string LiquidType = "OilLiquid";

        protected readonly ILiquidConfiguration Configuration;

        private readonly int ignitionTemperature;
        private readonly int heatSpeed;
        private readonly double burningRate;

        private int volume;

        public OilLiquidObject(int volume)
        {
            Configuration = ConfigurationManager.GetLiquidConfiguration(LiquidType);

            ignitionTemperature = GetCustomInt(CustomValueIgnitionTemperature);
            BurningTemperature = GetCustomInt(CustomValueBurningTemperature);
            heatSpeed = GetCustomInt(CustomValueHeatSpeed);
            burningRate = GetCustomDouble(CustomValueBurningRate);

            this.volume = volume;
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public ZIndex ZIndex => ZIndex.FloorCover;

        public ObjectSize Size => ObjectSize.Huge;

        public string Name => "Oil";

        public string Type => LiquidType;

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            var cell = map.GetCell(position);

            if (Volume <= 0)
            {
                map.RemoveObject(position, this);
                return;
            }

            if (cell.Temperature >= ignitionTemperature)
            {
                ProcessBurning(cell);
            }

            if (Volume >= MinVolumeForEffect)
            { 
                ApplyOilyStatus(cell, journal);
            }
        }

        private void ApplyOilyStatus(IAreaMapCell cell, IJournal journal)
        {
            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                destroyable.Statuses.Add(new OilyObjectStatus(Configuration), journal);
            }
        }

        private void ProcessBurning(IAreaMapCell cell)
        {
            if (cell.Temperature < BurningTemperature)
            {
                var temperatureDiff = BurningTemperature - cell.Temperature;
                var temperatureChange = Math.Min(temperatureDiff, heatSpeed);
                cell.Temperature += temperatureChange;
            }

            var burnedVolume = (int)Math.Ceiling(cell.Temperature * burningRate);
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

        public ISpreadingObject Separate(int separateVolume)
        {
            Volume -= separateVolume;
            return Injector.Current.Create<IOilLiquidObject>(separateVolume);
        }

        public bool Updated { get; set; }

        #region IMapObject Implementation

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => Volume >= MinVolumeForEffect;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }

        #endregion

        public bool GetIsOnFire(AreaMapCell cell)
        {
            return cell.Environment.Temperature >= ignitionTemperature;
        }

        public bool SpreadsFire => true;

        public int BurningTemperature { get; }
    }
}