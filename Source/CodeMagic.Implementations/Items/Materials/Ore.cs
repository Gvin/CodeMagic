using System;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Materials
{
    public class Ore : ItemBase, IDescriptionProvider, IWorldImageProvider, IInventoryImageProvider
    {
        public Ore(MetalType metalType)
        {
            MetalType = metalType;
        }

        public MetalType MetalType { get; }

        public override string Name
        {
            get
            {
                switch (MetalType)
                {
                    case MetalType.Copper:
                        return "Copper Ore";
                    case MetalType.Iron:
                        return "Iron Ore";
                    case MetalType.Silver:
                        return "Silver Ore";
                    case MetalType.ElvesMetal:
                        return "Elves Ore";
                    case MetalType.DwarfsMetal:
                        return "Dwarfs Ore";
                    case MetalType.Mythril:
                        return "Mythril Ore";
                    case MetalType.Adamant:
                        return "Adamant Ore";
                    default:
                        throw new ArgumentException($"Bad ore type: {MetalType}");
                }
            }
        }

        public override string Key
        {
            get
            {
                switch (MetalType)
                {
                    case MetalType.Copper:
                        return "resource_ore_copper";
                    case MetalType.Iron:
                        return "resource_ore_iron";
                    case MetalType.Silver:
                        return "resource_ore_silver";
                    case MetalType.ElvesMetal:
                        return "resource_ore_elves";
                    case MetalType.DwarfsMetal:
                        return "resource_ore_dwarfs";
                    case MetalType.Mythril:
                        return "resource_ore_mythril";
                    case MetalType.Adamant:
                        return "resource_ore_adamant";
                    default:
                        throw new ArgumentException($"Bad ore type: {MetalType}");
                }
            }
        }

        public override ItemRareness Rareness
        {
            get
            {
                switch (MetalType)
                {
                    case MetalType.Copper:
                        return ItemRareness.Common;
                    case MetalType.Iron:
                        return ItemRareness.Common;
                    case MetalType.Silver:
                        return ItemRareness.Uncommon;
                    case MetalType.ElvesMetal:
                        return ItemRareness.Uncommon;
                    case MetalType.DwarfsMetal:
                        return ItemRareness.Uncommon;
                    case MetalType.Mythril:
                        return ItemRareness.Rare;
                    case MetalType.Adamant:
                        return ItemRareness.Rare;
                    default:
                        throw new ArgumentException($"Bad ore type: {MetalType}");
                }
            }
        }

        public override int Weight
        {
            get
            {
                switch (MetalType)
                {
                    case MetalType.Copper:
                        return 4500;
                    case MetalType.Iron:
                        return 4000;
                    case MetalType.Silver:
                        return 5500;
                    case MetalType.ElvesMetal:
                        return 2500;
                    case MetalType.DwarfsMetal:
                        return 8000;
                    case MetalType.Mythril:
                        return 1400;
                    case MetalType.Adamant:
                        return 8000;
                    default:
                        throw new ArgumentException($"Bad ore type: {MetalType}");
                }
            }
        }

        public override bool Stackable => true;

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new[]
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {{"A chunk of ore. It can be used to get metal.", ItemTextHelper.DescriptionTextColor}}
            };
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("Decoratives_Stones_Small");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            var template = storage.GetImage("Item_Resource_OreTemplate");
            return MetalRecolorHelper.RecolorOreImage(template, MetalType);
        }
    }
}