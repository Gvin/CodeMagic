﻿using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Materials
{
    public class BlankScroll : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        public const string ItemKey = "scroll_blank";

        private const string WorldImageName = "ItemsOnGround_Other";
        private const string InventoryImageName = "Item_Scroll_Empty";

        public BlankScroll() : base(new ItemConfiguration
        {
            Name = "Blank Scroll",
            Key = ItemKey,
            Rareness = ItemRareness.Common,
            Weight = 300
        })
        {
        }

        public override bool Stackable => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(InventoryImageName);
        }

        public StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {"An empty parchment scroll."},
                new StyledLine {"A spell can be written to it with mana."},
                new StyledLine {"You will need double mana amount for this."}
            };
        }
    }
}