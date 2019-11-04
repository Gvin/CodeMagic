using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Tool;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Tool
{
    public abstract class ToolsGenerator
    {
        private readonly IToolConfiguration configuration;
        private readonly IImagesStorage imagesStorage;
        private readonly string itemName;
        private readonly string worldImageTemplate;

        public ToolsGenerator(IToolConfiguration configuration, IImagesStorage imagesStorage, string itemName, string worldImageTemplate)
        {
            this.configuration = configuration;
            this.imagesStorage = imagesStorage;
            this.itemName = itemName;
            this.worldImageTemplate = worldImageTemplate;
        }

        public IItem GenerateTool(ItemRareness rareness)
        {
            var config = configuration.RarenessConfiguration.FirstOrDefault(c => c.Rareness == rareness);
            if (config == null)
                throw new ApplicationException($"{itemName} tool configuration not found for rareness {rareness}");

            var material = RandomHelper.GetRandomElement(config.Materials);
            var inventoryImage = GetImage(configuration.InventoryImageTemplate, material);
            var worldImage = GetImage(worldImageTemplate, material);

            var name = $"{NameGenerationHelper.GetMaterialPrefix(material)} {itemName}";

            var maxDamage = GenerateMaxDamage(config);
            var minDamage = maxDamage.ToDictionary(pair => pair.Key, pair => pair.Value - config.MinMaxDamageDifference);
            var hitChance = RandomHelper.GetRandomValue(config.MinHitChance, config.MaxHitChance);
            var weight = GetWeight(material);
            var toolPower = RandomHelper.GetRandomValue(config.MinToolPower, config.MaxToolPower);
            var description = GenerateDescription();

            var itemConfig = GetConfiguration(toolPower);
            itemConfig.Name = name;
            itemConfig.Description = description;
            itemConfig.HitChance = hitChance;
            itemConfig.InventoryImage = inventoryImage;
            itemConfig.Key = Guid.NewGuid().ToString();
            itemConfig.Weight = weight;
            itemConfig.Rareness = rareness;
            itemConfig.MaxDamage = maxDamage;
            itemConfig.MinDamage = minDamage;
            itemConfig.WorldImage = worldImage;

            return CreateItem(itemConfig);
        }

        protected abstract WeaponItemConfiguration GetConfiguration(int toolPower);

        protected abstract IItem CreateItem(WeaponItemConfiguration config);

        protected abstract string[] GenerateDescription();

        private int GetWeight(ItemMaterial material)
        {
            var result = configuration.Weight.FirstOrDefault(config => config.Material == material);
            if (result == null)
                throw new ApplicationException($"No {itemName} weight configuration for material: {material}");

            return result.Weight;
        }

        private Dictionary<Element, int> GenerateMaxDamage(IToolRarenessConfiguration config)
        {
            return config.Damage.ToDictionary(pair => pair.Element,
                pair => RandomHelper.GetRandomValue(pair.MinValue, pair.MaxValue));
        }

        private SymbolsImage GetImage(string templateName, ItemMaterial material)
        {
            var template = imagesStorage.GetImage(templateName);
            return ItemRecolorHelper.RecolorItemImage(template, material);
        }
    }
}