using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.Objects.Buildings
{
    public class Fireplace : IWorldImageProvider, IUsableObject, IFuelObject
    {
        private const int DefaultMaxFuelCount = 5;

        public const string BuildingKey = "fireplace";
        private readonly List<IFuelItem> fuel;

        public event EventHandler Used;

        public Fireplace()
        {
            fuel = new List<IFuelItem>(MaxFuelCount);
        }

        public IFuelItem[] Fuel => fuel.ToArray();

        public void AddFuel(IFuelItem fuelItem)
        {
            if (fuel.Count == MaxFuelCount)
                throw new ApplicationException("Max fuel count reached.");

            fuel.Add(fuelItem);
        }

        public int MaxFuelCount => DefaultMaxFuelCount;

        public IFuelItem RemoveLastFuel()
        {
            var item = fuel.Last();
            fuel.Remove(item);
            return item;
        }
        public string Name => "Fireplace";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;
        public bool BlocksAttack => false;
        public bool BlocksEnvironment => false;
        public ZIndex ZIndex => ZIndex.AreaDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            if (fuel.Count > 0)
                return storage.GetImage("Building_Fireplace_Fuel");

            return storage.GetImage("Building_Fireplace_NoFuel");
        }

        public void Use(GameCore<Player> game, Point position)
        {
            Used?.Invoke(this, EventArgs.Empty);
        }

        public bool CanIgnite => Fuel.Length > 0;
        public int FuelLeft
        {
            get => Fuel.First().FuelLeft;
            set => Fuel.First().FuelLeft = value;
        }

        public int BurnTemperature => Fuel.First().BurnTemperature;
        public int IgnitionTemperature => Fuel.First().IgnitionTemperature;
    }
}