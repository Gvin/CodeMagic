﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.Implementations.Items;
using CodeMagic.ItemsGeneration.Configuration;
using CodeMagic.ItemsGeneration.Configuration.Armor;
using CodeMagic.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.UI.Images;

namespace CodeMagic.ItemsGeneration.Implementations
{
    public class ArmorGenerator
    {
        private readonly IArmorConfiguration configuration;
        private readonly IImagesStorage imagesStorage;
        private readonly BonusesGenerator bonusesGenerator;

        public ArmorGenerator(IArmorConfiguration configuration, BonusesGenerator bonusesGenerator, IImagesStorage imagesStorage)
        {
            this.configuration = configuration;
            this.imagesStorage = imagesStorage;
            this.bonusesGenerator = bonusesGenerator;
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
            var description = GenerateDescription(rareness, material);
            var weight = GetWeight(config, material);

            var bonusesCount = RandomHelper.GetRandomValue(rarenessConfig.MinBonuses, rarenessConfig.MaxBonuses);
            var itemConfig = new ArmorItemImplConfiguration
            {
                Name = name,
                Key = Guid.NewGuid().ToString(),
                ArmorType = armorType,
                Description = description,
                Image = image,
                Protection = protection,
                Rareness = rareness,
                Weight = weight
            };
            bonusesGenerator.GenerateBonuses(itemConfig, bonusesCount);

            return new ArmorItemImpl(itemConfig);
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

        private string[] GenerateDescription(ItemRareness rareness, ItemMaterial material)
        {
            return new[]
            {
                GetMaterialDescription(material),
                GetRarenessDescription(rareness)
            };
        }

        private string GetMaterialDescription(ItemMaterial material)
        {
            var textConfig = configuration.DescriptionConfiguration.MaterialDescription.FirstOrDefault(config => config.Material == material);
            if (textConfig == null)
                throw new ApplicationException($"Text configuration not found for armor material: {material}");

            return RandomHelper.GetRandomElement(textConfig.Text);
        }

        private string GetRarenessDescription(ItemRareness rareness)
        {
            var textConfig = configuration.DescriptionConfiguration.RarenessDescription.FirstOrDefault(config => config.Rareness == rareness);
            if (textConfig == null)
                throw new ApplicationException($"Text configuration not found for armor rareness: {rareness}");

            return RandomHelper.GetRandomElement(textConfig.Text);
        }

        private string GenerateName(ItemRareness rareness, ItemMaterial material, string typeName, ArmorType type)
        {
            var materialPrefix = NameGenerationHelper.GetMaterialPrefix(material);
            var armorTypeName = GetArmorTypeName(type);

            var builder = new List<string>();

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