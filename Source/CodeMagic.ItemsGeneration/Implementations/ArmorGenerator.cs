using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.Implementations.Items;
using CodeMagic.ItemsGeneration.Configuration;
using CodeMagic.ItemsGeneration.Configuration.Armor;
using CodeMagic.UI.Images;

namespace CodeMagic.ItemsGeneration.Implementations
{
    public class ArmorGenerator
    {
        private readonly IArmorConfiguration configuration;
        private readonly IImagesStorage imagesStorage;

        public ArmorGenerator(IArmorConfiguration configuration, IImagesStorage imagesStorage)
        {
            this.configuration = configuration;
            this.imagesStorage = imagesStorage;
        }

        public ArmorItem GenerateArmor(ItemRareness rareness)
        {
            var armorType = GenerateArmorType();
            var config = GetSpecificConfiguration(armorType);
            var rarenessConfig = GetRarenessConfiguration(config, rareness);
            var material = RandomHelper.GetRandomElement(rarenessConfig.Materials);
            var image = GetArmorImage(config, material);
            var protection = GenerateProtection(rarenessConfig.Protection);
            var name = GenerateName(rareness, material, config.TypeName, armorType);
            var description = GenerateDescription();
            var weight = GetWeight(config, material);

            return new ArmorItemImpl(new ArmorItemImplConfiguration
            {
                Name = name,
                Key = Guid.NewGuid().ToString(),
                ArmorType =  armorType,
                Description = description,
                Image = image,
                Protection = protection,
                Rareness = rareness,
                Weight = weight
            });
        }

        private string GetArmorTypeName(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.Helmet:
                    return "Helmet";
                case ArmorType.Chest:
                    return "Chest";
                case ArmorType.Leggings:
                    return "Leggings";
                default:
                    throw new ArgumentException($"Unknown armor type: {armorType}");
            }
        }

        private int GetWeight(IArmorPieceConfiguration config, ItemMaterial material)
        {
            var result = config.Weight.FirstOrDefault(w => w.Material == material);
            if (result == null)
                throw new ApplicationException($"Weight configuration not found for armor material {material}");
            return result.Weight;
        }

        private string[] GenerateDescription()
        {
            return new[] {"Armor description."};
        }

        private string GenerateName(ItemRareness rareness, ItemMaterial material, string typeName, ArmorType type)
        {
            var rarenessPrefix = NameGenerationHelper.GetRarenessPrefix(rareness);
            var materialPrefix = NameGenerationHelper.GetMaterialPrefix(material);
            var armorTypeName = GetArmorTypeName(type);

            var builder = new List<string>();
            if (!string.IsNullOrEmpty(rarenessPrefix))
            {
                builder.Add(rarenessPrefix);
            }

            if (!string.IsNullOrEmpty(materialPrefix))
            {
                builder.Add(materialPrefix);

            }

            if (!string.IsNullOrEmpty(typeName))
            {
                builder.Add(typeName);
            }

            builder.Add(armorTypeName);

            return string.Join(" ", builder);
        }

        private Dictionary<Element, int> GenerateProtection(IElementConfiguration[] config)
        {
            return config.ToDictionary(c => c.Element, c => RandomHelper.GetRandomValue(c.MinValue, c.MaxValue));
        }

        private IArmorRarenessConfiguration GetRarenessConfiguration(IArmorPieceConfiguration config,
            ItemRareness rareness)
        {
            var result = config.RarenessConfigurations.FirstOrDefault(c => c.Rareness == rareness);
            if (result == null)
                throw new ApplicationException($"Rareness config not found for armor rareness {rareness}");

            return result;
        }

        private SymbolsImage GetArmorImage(IArmorPieceConfiguration config, ItemMaterial material)
        {
            var imageName = RandomHelper.GetRandomElement(config.Images);
            var imageInit = imagesStorage.GetImage(imageName);
            return ItemRecolorHelper.RecolorImage(imageInit, material);
        }

        private IArmorPieceConfiguration GetSpecificConfiguration(ArmorType type)
        {
            var configurations = GetArmorTypeConfigurations(type);
            return RandomHelper.GetRandomElement(configurations);
        }

        private IArmorPieceConfiguration[] GetArmorTypeConfigurations(ArmorType type)
        {
            switch (type)
            {
                case ArmorType.Helmet:
                    return configuration.HelmetConfiguration;
                case ArmorType.Chest:
                    return configuration.ChestConfiguration;
                case ArmorType.Leggings:
                    return configuration.LeggingsConfiguration;
                default:
                    throw new ArgumentException($"Unknown armor type: {type}");
            }
        }

        private ArmorType GenerateArmorType()
        {
            return (ArmorType) RandomHelper.GetRandomValue(0, 2);
        }
    }
}