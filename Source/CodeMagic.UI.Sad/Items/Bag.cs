using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Views;

namespace CodeMagic.UI.Sad.Items
{
    public class Bag : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider, IUsableItem
    {
        private const int MaxWeight = 7000;

        private readonly Inventory inventory;

        public Bag()
        {
            inventory = new Inventory();
        }

        public override string Name => "Bag";

        public override string Key => "bag";

        public override ItemRareness Rareness => ItemRareness.Common;

        public override int Weight => 2000 + inventory.GetWeight();

        public override bool Stackable => false;

        public bool Use(GameCore<Player> game)
        {
            var view = new StorageInventoryView(game, Name, inventory, MaxWeight, this);
            view.Show();

            return true;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Other");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Item_Bag");
        }

        public StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine{{"A bag made of good leather.", TextHelper.DescriptionTextColor}}
            };
        }
    }
}