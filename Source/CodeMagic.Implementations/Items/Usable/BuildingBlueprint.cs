﻿using CodeMagic.Core.Configuration.Buildings;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Usable
{
    public class BuildingBlueprint : ItemBase, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private readonly IBuildingConfiguration building;

        public BuildingBlueprint(IBuildingConfiguration building)
        {
            this.building = building;
        }

        public override string Name => $"{building.Name} Blueprint";

        public override string Key => "building_blueprint";

        public override ItemRareness Rareness => building.Rareness;

        public override int Weight => 500;

        public override bool Stackable => false;

        public bool Use(IGameCore game)
        {
            if (game.Player.UnlockBuilding(building))
            {
                game.Journal.Write(new BuildingUnlockedMessage(building));
            }
            return false;
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Other");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Item_Blueprint");
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine
                    {new StyledString($"Blueprint for {building.Name} building.", ItemTextHelper.DescriptionTextColor)}
            };
        }
    }
}