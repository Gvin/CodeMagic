using System.Collections.Generic;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable
{
    public class TeleporterStoneActive : Item, IUsableItem, IDescriptionProvider, IWorldImageProvider, IInventoryImageProvider
    {
        private const string InventoryImageActive = "Item_TeleporterStone_Active";

        private readonly string locationId;
        private readonly string locationName;
        private readonly Point position;

        public TeleporterStoneActive(string locationId, string locationName, Point position)
        {
            this.locationId = locationId;
            this.locationName = locationName;
            this.position = position;
        }

        public override string Name => $"Teleporter Stone ({locationName})";

        public override string Key => "teleporter_stone_active";

        public override ItemRareness Rareness => ItemRareness.Uncommon;

        public override int Weight => 500;

        public override bool Stackable => false;

        public bool Use(IGameCore game)
        {
            game.World.TravelToLocation(game, locationId, Direction.North, position);
            return false;
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new List<StyledLine>
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {$"It is bound to \"{locationName}\" [{position.X}; {position.Y}]"},
                StyledLine.Empty,
                new StyledLine {new StyledString("A single-use item which can be used", ItemTextHelper.DescriptionTextColor)},
                new StyledLine {new StyledString("to teleport to the stored location.", ItemTextHelper.DescriptionTextColor)},
                new StyledLine {new StyledString("The first usage will save current location.", ItemTextHelper.DescriptionTextColor)},
                new StyledLine {new StyledString("The second usage will teleport to the stored location.", ItemTextHelper.DescriptionTextColor)}
            }.ToArray();
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Other");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(InventoryImageActive);
        }
    }
}