using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings
{
    public class GrowingPlace : IMapObject, IWorldImageProvider, IDynamicObject
    {
        public const string BuildingKey = "growing_place";

        private double humidity;
        private const double MaxHumidity = 100d;
        private const double HumidityDecrease = 0.001;
        private const int MaxWaterConsumption = 1;

        public GrowingPlace()
        {
            Humidity = MaxHumidity * 0.3d;
        }

        public string Name => "Growing Place";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;
        public bool BlocksAttack => false;
        public bool BlocksEnvironment => false;
        public ZIndex ZIndex => ZIndex.FloorCover;

        public double Humidity
        {
            get => humidity;
            set => humidity = Math.Min(MaxHumidity, Math.Max(0, value));
        }

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var imageName = GetImageName();
            return storage.GetImage(imageName);
        }

        private string GetImageName()
        {
            if (Humidity >= 90)
                return "Building_GrowingPlace_OverHumid";
            if (Humidity >= 70)
                return "Building_GrowingPlace_VeryHumid";
            if (Humidity >= 50)
                return "Building_GrowingPlace_Humid";
            if (Humidity >= 30)
                return "Building_GrowingPlace_MediumHumid";
            if (Humidity >= 10)
                return "Building_GrowingPlace_LittleHumid";

            return "Building_GrowingPlace_Dry";
        }

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            var cell = map.GetCell(position);
            var waterVolume = cell.GetVolume<IWaterLiquidObject>();
            if (waterVolume > 0)
            {
                var possibleConsumption = Math.Min(MaxWaterConsumption, waterVolume);
                var consumption = Math.Min(possibleConsumption, (int)Math.Floor(MaxHumidity - Humidity));
                cell.RemoveVolume<IWaterLiquidObject>(consumption);
                Humidity += consumption;
            }

            Humidity -= HumidityDecrease;
        }

        public bool Updated { get; set; }
        public UpdateOrder UpdateOrder => UpdateOrder.Late;
    }
}