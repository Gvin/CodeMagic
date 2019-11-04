using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings
{
    public class WoodenFloor : IWorldImageProvider, IFuelObject
    {
        public const string BuildingKey = "wooden_floor";

        private const int MaxFuel = 60;

        public WoodenFloor()
        {
            FuelLeft = MaxFuel;
        }

        public string Name => "Wooden Floor";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;
        public bool BlocksAttack => false;
        public bool BlocksEnvironment => false;
        public ZIndex ZIndex => ZIndex.FloorCover;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("Building_WoodenFloor");
        }

        public bool CanIgnite => true;
        public int FuelLeft { get; set; }
        public int BurnTemperature => 700;
        public int IgnitionTemperature => 450;
    }
}