﻿using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Materials
{
    public class Glass : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        public const string ResourceKey = "resource_glass";

        public override string Name => "Glass";

        public override string Key => ResourceKey;

        public override ItemRareness Rareness => ItemRareness.Common;

        public override int Weight => 2000;

        public override bool Stackable => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Other");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Item_Resource_Glass");
        }

        public StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {{"A sheet of glass.", ItemTextHelper.DescriptionTextColor}},
                new StyledLine {{"It can be used for building.", ItemTextHelper.DescriptionTextColor}}
            };
        }
    }
}