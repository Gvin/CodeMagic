using System;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Materials
{
    public class Ingot : ItemBase, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string TemplateImageName = "Item_Resource_IngotTemplate";

        public Ingot(MetalType metalType)
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
                        return "Copper Ingot";
                    case MetalType.Bronze:
                        return "Bronze Ingot";
                    case MetalType.Iron:
                        return "Iron Ingot";
                    case MetalType.Steel:
                        return "Steel Ingot";
                    case MetalType.Silver:
                        return "Silver Ingot";
                    case MetalType.ElvesMetal:
                        return "Elves Metal Ingot";
                    case MetalType.DwarfsMetal:
                        return "Dwarfs Metal Ingot";
                    case MetalType.Mythril:
                        return "Mythril Ingot";
                    case MetalType.Adamant:
                        return "Adamant Ingot";
                    default:
                        throw new ArgumentException($"Unknown metal type { MetalType}");
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
                        return "resource_ingot_copper";
                    case MetalType.Bronze:
                        return "resource_ingot_bronze";
                    case MetalType.Iron:
                        return "resource_ingot_iron";
                    case MetalType.Steel:
                        return "resource_ingot_steel";
                    case MetalType.Silver:
                        return "resource_ingot_silver";
                    case MetalType.ElvesMetal:
                        return "resource_ingot_elves";
                    case MetalType.DwarfsMetal:
                        return "resource_ingot_dwarfs";
                    case MetalType.Mythril:
                        return "resource_ingot_mythril";
                    case MetalType.Adamant:
                        return "resource_ingot_adamant";
                    default:
                        throw new ArgumentException($"Unknown metal type { MetalType}");
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
                    case MetalType.Bronze:
                        return ItemRareness.Common;
                    case MetalType.Iron:
                        return ItemRareness.Common;
                    case MetalType.Steel:
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
                        throw new ArgumentException($"Unknown metal type { MetalType}");
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
                        return 9000;
                    case MetalType.Bronze:
                        return 7500;
                    case MetalType.Iron:
                        return 8000;
                    case MetalType.Steel:
                        return 8000;
                    case MetalType.Silver:
                        return 10500;
                    case MetalType.ElvesMetal:
                        return 5000;
                    case MetalType.DwarfsMetal:
                        return 16000;
                    case MetalType.Mythril:
                        return 2700;
                    case MetalType.Adamant:
                        return 16000;
                    default:
                        throw new ArgumentException($"Unknown metal type { MetalType}");
                }
            }
        }

        public sealed override bool Stackable => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage("ItemsOnGround_Other");
        }

        public SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            var template = storage.GetImage(TemplateImageName);
            return MetalRecolorHelper.RecolorMetalImage(template, MetalType);
        }

        public StyledLine[] GetDescription(IPlayer player)
        {
            return new []
            {
                ItemTextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine{new StyledString("Standard metal ingot.", ItemTextHelper.DescriptionTextColor)} 
            };
        }
    }
}