using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Materials
{
    public class Wood : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider, IFuelItem
    {
        public const string ResourceKey = "resource_wood";

        private const int DefaultMaxFuel = 120;

        public Wood()
        {
            FuelLeft = MaxFuel;
        }

        public override string Name => "Wood";

        public override string Key => ResourceKey;

        public override ItemRareness Rareness => ItemRareness.Common;

        public override int Weight => 2000;

        public override bool Stackable => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Resource_Wood");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Item_Resource_Wood");
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {$"Fuel: {FuelLeft} / {MaxFuel}"},
                StyledLine.Empty,
                new StyledLine {{"A big piece of wood.", ItemTextHelper.DescriptionTextColor}},
                new StyledLine {{"It can be used for building or as a fuel source.", ItemTextHelper.DescriptionTextColor}}
            };
        }

        public bool CanIgnite => true;

        public int FuelLeft { get; set; }

        public int MaxFuel => DefaultMaxFuel;

        public int BurnTemperature => 700;

        public int IgnitionTemperature => 450;
    }
}