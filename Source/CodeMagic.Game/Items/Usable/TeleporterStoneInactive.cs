using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Game.JournalMessages;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable
{
    public class TeleporterStoneInactive : Item, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string InventoryImageInactive = "Item_TeleporterStone_Inactive";

        public bool Use(IGameCore game)
        {
            if (!game.World.CurrentLocation.KeepOnLeave)
            {
                game.Journal.Write(new StoringLocationNotAllowedMessage());
                return true;
            }

            var locationId = game.World.CurrentLocation.Id;
            var locationName = game.World.CurrentLocation.Name;
            var position = game.PlayerPosition;
            game.Player.Inventory.AddItem(new TeleporterStoneActive(locationId, locationName, position));
            return false;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Other");
        }

        public override string Name => "Teleporter Stone";

        public override string Key => "teleporter_stone_inactive";

        public override ItemRareness Rareness => ItemRareness.Uncommon;

        public override int Weight => 500;

        public override bool Stackable => true;

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(InventoryImageInactive);
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new List<StyledLine>
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {new StyledString("A single-use item which can be used", ItemTextHelper.DescriptionTextColor)},
                new StyledLine {new StyledString("to teleport to the stored location.", ItemTextHelper.DescriptionTextColor)},
                new StyledLine {new StyledString("The first usage will save current location.", ItemTextHelper.DescriptionTextColor)},
                new StyledLine {new StyledString("The second usage will teleport to the stored location.", ItemTextHelper.DescriptionTextColor)}
            }.ToArray();
        }
    }
}