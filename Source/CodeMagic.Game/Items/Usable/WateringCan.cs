using System;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Game.Objects.Buildings;
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

        public bool Use(IGameCore game)
        {
            var targetPosition = Point.GetPointInDirection(game.PlayerPosition, game.Player.Direction);
            var cell = game.Map.TryGetCell(targetPosition);
            if (cell != null)
            {
                UseCan(cell);
            }

            return true;
        }

        private void UseCan(IAreaMapCell cell)
        {
            var growingPlace = cell.Objects.OfType<GrowingPlace>().FirstOrDefault();
            if (growingPlace != null && canWaterLevel > 0)
            {
                growingPlace.Humidity += canWaterLevel;
                canWaterLevel = 0;
                return;
            }

            var waterVolume = cell.GetVolume<IWaterLiquidObject>();
            if (waterVolume > 0 && canWaterLevel < MaxWaterLevel)
            {
                var waterToFill = Math.Min(waterVolume, MaxWaterLevel - canWaterLevel);
                canWaterLevel += waterToFill;
                cell.RemoveVolume<IWaterLiquidObject>(waterToFill);
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

        public StyledLine[] GetDescription(IPlayer player)
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