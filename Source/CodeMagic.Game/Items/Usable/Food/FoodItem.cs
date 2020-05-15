using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable.Food
{
    public class FoodItem : Item, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string SaveKeyHungerDecrease = "HungerDecrease";
        private const string SaveKeyDescription = "Description";
        private const string SaveKeyInventoryImage = "InventoryImage";
        private const string SaveKeyWorldImage = "WorldImage";

        private readonly int hungerDecrease;
        private readonly string[] description;
        private readonly SymbolsImage inventoryImage;
        private readonly SymbolsImage worldImage;

        public FoodItem(SaveData data)
            : base(data)
        {
            hungerDecrease = data.GetIntValue(SaveKeyHungerDecrease);
            description = data.GetValuesCollection(SaveKeyDescription);
            inventoryImage = data.GetObject<SymbolsImageSaveable>(SaveKeyInventoryImage).GetImage();
            worldImage = data.GetObject<SymbolsImageSaveable>(SaveKeyWorldImage).GetImage();
        }

        public FoodItem(FoodItemConfiguration configuration)
            : base(configuration)
        {
            hungerDecrease = configuration.HungerDecrease;
            description = configuration.Description;
            inventoryImage = configuration.InventoryImage;
            worldImage = configuration.WorldImage;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyHungerDecrease, hungerDecrease);
            data.Add(SaveKeyDescription, description);
            data.Add(SaveKeyWorldImage, new SymbolsImageSaveable(worldImage));
            data.Add(SaveKeyInventoryImage, new SymbolsImageSaveable(inventoryImage));
            return data;
        }

        public bool Use(CurrentGame.GameCore<Player> game)
        {
            game.Player.HungerPercent -= hungerDecrease;
            game.Journal.Write(new HungerDecreasedMessage(hungerDecrease));
            return false;
        }

        public override bool Stackable => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return worldImage;
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return inventoryImage;
        }

        public StyledLine[] GetDescription(Player player)
        {
            var result = new List<StyledLine>
            {
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {"Hunger Decrease: ", TextHelper.GetValueString(hungerDecrease, "%", false)},
                StyledLine.Empty
            };
            result.AddRange(TextHelper.ConvertDescription(description));
            return result.ToArray();
        }
    }

    public class FoodItemConfiguration : ItemConfiguration
    {
        public int HungerDecrease { get; set; }

        public string[] Description { get; set; }

        public SymbolsImage InventoryImage { get; set; }

        public SymbolsImage WorldImage { get; set; }
    }
}