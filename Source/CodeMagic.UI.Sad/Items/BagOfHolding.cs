using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Views;

namespace CodeMagic.UI.Sad.Items
{
    public class BagOfHolding : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider, IUsableItem
    {
        private Point position;
        private readonly Inventory inventory;

        public BagOfHolding()
        {
            inventory = new Inventory();
        }

        public override string Name => "Bag of Holding";

        public override string Key => "bag_of_holding";

        public override ItemRareness Rareness => ItemRareness.Rare;

        public override int Weight => 2000;

        public override bool Stackable => false;

        public bool Use(IGameCore game)
        {
            if (position == null)
            {
                if (!string.Equals(game.World.CurrentLocation.Id, "home"))
                {
                    game.Journal.Write(new StoringLocationNotAllowedMessage());
                    return true;
                }

                position = game.PlayerPosition;
                return true;
            }

            var view = new StorageInventoryView(game, Name, inventory, null, this);
            view.Closed += (sender, args) => TeleportItems(game);
            view.Show();

            return true;
        }

        private void TeleportItems(IGameCore game)
        {
            var location = game.World.GetLocation("home");

            foreach (var inventoryStack in inventory.Stacks)
            {
                foreach (var item in inventoryStack.Items)
                {
                    location.CurrentArea.AddObject(position, item);
                }
            }

            inventory.Clear();
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Other");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            if (position == null)
            {
                return storage.GetImage("Item_BagOfHolding_Inactive");
            }
            return storage.GetImage("Item_BagOfHolding_Active");
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine{{"A bag made of good leather.", ItemTextHelper.DescriptionTextColor}},
                new StyledLine{{"There are a lot of runes on it, you feel strong magic in them.", ItemTextHelper.DescriptionTextColor}},
                new StyledLine{{"This bag can be used to teleport some items to your home.", ItemTextHelper.DescriptionTextColor}}
            };
        }
    }
}