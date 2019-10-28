using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings.Plants
{
    public abstract class PlantBase : DestroyableObject, IWorldImageProvider, IDynamicObject, IUsableObject, IPlant
    {
        protected PlantBase(int maxHealth)
            : base(maxHealth)
        {
            GrowthPeriod = 0;
        }

        protected abstract double HumidityConsumption { get; }

        protected abstract double MaxHumidity { get; }

        protected abstract double MinHumidity { get; }

        protected abstract double MinTemperature { get; }

        protected abstract double MaxTemperature { get; }

        protected int GrowthPeriod { get; set; }

        public override ZIndex ZIndex => ZIndex.GroundDecoration;

        public override ObjectSize Size => ObjectSize.Huge;

        public abstract SymbolsImage GetWorldImage(IImagesStorage storage);

        public virtual void Update(IAreaMap map, IJournal journal, Point position)
        {
            var cell = map.GetCell(position);
            var growingPlace = cell.Objects.OfType<GrowingPlace>().FirstOrDefault();
            if (growingPlace == null)
                throw new ApplicationException("Growing place not found.");

            CheckHumidity(growingPlace);
            CheckTemperature(cell);
        }

        private void CheckTemperature(IAreaMapCell cell)
        {
            if (cell.Temperature > MaxTemperature || cell.Temperature < MinTemperature)
            {
                Health--;
            }
        }

        private void CheckHumidity(GrowingPlace growingPlace)
        {
            if (growingPlace.Humidity > MaxHumidity || growingPlace.Humidity < MinHumidity)
            {
                Health--;
                return;
            }

            growingPlace.Humidity -= HumidityConsumption;
            GrowthPeriod++;
        }

        public bool Updated { get; set; }
        public UpdateOrder UpdateOrder => UpdateOrder.Late;
        public abstract void Use(GameCore<Player> game, Point position);
    }
}