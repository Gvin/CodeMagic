using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Buildings;
using CodeMagic.Game.Objects.Buildings.Plants;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable.Seeds
{
    public class WheatSeeds : Item, IUsableItem, IInventoryImageProvider, IWorldImageProvider, IDescriptionProvider
    {
        public override string Name => "Wheat Seeds";
        public override string Key => "seeds_wheat";
        public override ItemRareness Rareness => ItemRareness.Common;
        public override int Weight => 100;
        public override bool Stackable => true;

        public bool Use(GameCore<Player> game)
        {
            var targetPoint = Point.GetPointInDirection(game.PlayerPosition, game.Player.Direction);
            var targetCell = game.Map.TryGetCell(targetPoint);
            if (targetCell == null || 
                !targetCell.Objects.OfType<GrowingPlace>().Any() ||
                targetCell.Objects.OfType<IPlant>().Any())
            {
                game.Journal.Write(new BadPlantingPlaceMessage());
                return true;
            }

            game.Map.AddObject(targetPoint, new WheatPlant());
            return false;
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Seeds_Wheat");
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_WheatSeeds");
        }

        public StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {{"Can be used to plant wheat.", ItemTextHelper.DescriptionTextColor}}
            };
        }
    }
}