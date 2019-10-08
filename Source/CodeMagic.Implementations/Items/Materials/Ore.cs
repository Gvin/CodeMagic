using System;
using System.Collections.Generic;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items.Materials
{
    public class Ore : ItemBase, IDescriptionProvider, IWorldImageProvider, IInventoryImageProvider, IFurnaceItem
    {
        private static readonly Dictionary<MetalType, OreData> OreTypes = new Dictionary<MetalType, OreData>
        {
            {MetalType.Copper, new OreData("Copper Ore", "resource_ore_copper", 9000, ItemRareness.Common, 1000, 2550, 20)},
            {MetalType.Iron, new OreData("Iron Ore", "resource_ore_iron", 8000, ItemRareness.Common, 1500, 2850, 30)},
            {MetalType.Silver, new OreData("Silver Ore", "resource_ore_silver", 11000, ItemRareness.Uncommon, 950, 2150, 35)},
            {MetalType.ElvesMetal, new OreData("Elves Ore", "resource_ore_elves", 5000, ItemRareness.Uncommon, 1500, 2850, 50)},
            {MetalType.DwarfsMetal, new OreData("Dwarfs Ore", "resource_ore_dwarfs", 16000, ItemRareness.Uncommon, 1800, 3000, 55)},
            {MetalType.Mythril, new OreData("Mythril Ore", "resource_ore_mythril", 3000, ItemRareness.Rare, 660, 2470, 80)},
            {MetalType.Adamant, new OreData("Adamant Ore", "resource_ore_adamant", 16000, ItemRareness.Rare, 2870, 6000, 150)}
        };

        public Ore(MetalType metalType)
        {
            MetalType = metalType;
        }

        public MetalType MetalType { get; }

        public override string Name => Data.Name;

        private OreData Data
        {
            get
            {
                if (OreTypes.ContainsKey(MetalType))
                    return OreTypes[MetalType];

                throw new ArgumentException($"Bad ore type: {MetalType}");
            }
        }

        public override string Key => Data.Key;

        public override ItemRareness Rareness => Data.Rareness;

        public override int Weight => Data.Weight;

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

        public int MinTemperature => Data.MinTemperature;
        public int MaxTemperature => Data.MaxTemperature;
        public int FurnaceProcessingTime => Data.FurnaceProcessingType;
        public IItem CreateFurnaceResult()
        {
            return new Ingot(MetalType);
        }

        private class OreData
        {
            public OreData(string name, string key, int weight, ItemRareness rareness, int minTemperature, int maxTemperature, int furnaceProcessingType)
            {
                Name = name;
                Key = key;
                Weight = weight;
                Rareness = rareness;
                MinTemperature = minTemperature;
                MaxTemperature = maxTemperature;
                FurnaceProcessingType = furnaceProcessingType;
            }

            public string Name { get; }

            public string Key { get; }

            public int Weight { get; }

            public ItemRareness Rareness { get; }

            public int MinTemperature { get; }

            public int MaxTemperature { get; }

            public int FurnaceProcessingType { get; }
        }
    }
}