﻿using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Materials
{
    public class BlankScroll : Item, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        public const string ItemKey = "scroll_blank";

        private const string WorldImageName = "ItemsOnGround_Scroll";
        private const string InventoryImageName = "Item_Scroll_Empty";

        public BlankScroll(SaveData data) 
            : base(data)
        {
        }

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
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {new StyledString("An empty parchment scroll.", TextHelper.DescriptionTextColor) },
                new StyledLine {new StyledString("A spell can be written on it with mana.", TextHelper.DescriptionTextColor) },
                new StyledLine {new StyledString("You will need double mana amount for this.", TextHelper.DescriptionTextColor) }
            };
        }
    }
}