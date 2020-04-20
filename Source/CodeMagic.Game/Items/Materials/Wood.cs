using System.Collections.Generic;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Materials
{
    public class Wood : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider, IFuelItem
    {
        private const string SaveKeyFuelLeft = "FuelLeft";

        private const string ResourceKey = "resource_wood";

        private const int DefaultMaxFuel = 120;

        public Wood(SaveData data)
            : base(data)
        {
            FuelLeft = data.GetIntValue(SaveKeyFuelLeft);
        }

        public Wood()
            : base(new ItemConfiguration
            {
                Key = ResourceKey,
                Name = "Wood",
                Rareness = ItemRareness.Trash,
                Weight = 2000
            })
        {
            FuelLeft = MaxFuel;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyFuelLeft, FuelLeft);
            return data;
        }

        public override bool Stackable => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Resource_Wood");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Item_Resource_Wood");
        }

        public StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {{"A big piece of wood.", TextHelper.DescriptionTextColor}},
                new StyledLine {{"It can be used as a fuel source.", TextHelper.DescriptionTextColor}}
            };
        }

        public bool CanIgnite => true;

        public int FuelLeft { get; set; }

        public int MaxFuel => DefaultMaxFuel;

        public int BurnTemperature => 700;

        public int IgnitionTemperature => 450;
    }
}