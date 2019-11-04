using System;
using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Objects.LiquidObjects;
using CodeMagic.UI.Images;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.Items.Usable
{
    public class WateringCan : Item, IUsableItem, IInventoryImageProvider, IWorldImageProvider, IDescriptionProvider
    {
        private const int MaxWaterLevel = 10;

        private int canWaterLevel;

        public WateringCan()
        {
            canWaterLevel = 0;
        }

        public override string Name => "Watering Can";
        public override string Key => "watering_can";
        public override ItemRareness Rareness => ItemRareness.Common;
        public override int Weight => 3000 + canWaterLevel * 1000;
        public override bool Stackable => false;

        public bool Use(GameCore<Player> game)
        {
            var targetPosition = Point.GetPointInDirection(game.PlayerPosition, game.Player.Direction);
            UseCan(game.Map, targetPosition);

            return true;
        }

        private void UseCan(IAreaMap map, Point position)
        {
            var cell = map.TryGetCell(position);
            if (cell == null)
                return;

            if (canWaterLevel > 0)
            {
                map.AddObject(position, new WaterLiquid(canWaterLevel));
                canWaterLevel = 0;
                return;
            }

            var waterVolume = cell.GetVolume<WaterLiquid>();
            if (waterVolume > 0 && canWaterLevel < MaxWaterLevel)
            {
                var waterToFill = Math.Min(waterVolume, MaxWaterLevel - canWaterLevel);
                canWaterLevel += waterToFill;
                cell.RemoveVolume<WaterLiquid>(waterToFill);
            }
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Item_WateringCan");
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Item_WateringCan");
        }

        public StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {"Water Level: ", {$"{canWaterLevel} / {MaxWaterLevel}", Color.CadetBlue}},
                StyledLine.Empty,
                new StyledLine {{"A can which can be used to water plants.", ItemTextHelper.DescriptionTextColor}},
                new StyledLine {{"It can be refilled with any water source.", ItemTextHelper.DescriptionTextColor}}
            };
        }
    }
}