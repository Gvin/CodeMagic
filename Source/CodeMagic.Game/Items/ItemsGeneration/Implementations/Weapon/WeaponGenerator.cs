﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Weapon
{
    internal class WeaponGenerator : IWeaponGenerator
    {
        private const int MaxDurabilityPercent = 100;
        private const int MinDurabilityPercent = 30;

        private readonly IImagesStorage imagesStorage;
        private readonly string baseName;
        private readonly string worldImageName;
        private readonly IWeaponConfiguration configuration;
        private readonly IWeaponsConfiguration configurations;
        private readonly BonusesGenerator bonusesGenerator;

        public WeaponGenerator(
            string baseName, 
            string worldImageName,
            IWeaponConfiguration configuration,
            IWeaponsConfiguration configurations, 
            BonusesGenerator bonusesGenerator, 
            IImagesStorage imagesStorage)
        {
            this.configurations = configurations;
            this.imagesStorage = imagesStorage;
            this.bonusesGenerator = bonusesGenerator;
            this.baseName = baseName;
            this.worldImageName = worldImageName;
            this.configuration = configuration;
        }

        public WeaponItem GenerateWeapon(ItemRareness rareness)
        {
            var rarenessConfiguration = GetRarenessConfiguration(rareness);
            var material = RandomHelper.GetRandomElement(rarenessConfiguration.Materials);
            var inventoryImage = GenerateImage(material);
            var worldImage = GetMaterialColoredImage(worldImageName, material);
            var equippedRightImage = GetMaterialColoredImage(configuration.EquippedImageRight, material);
            var equippedLeftImage = GetMaterialColoredImage(configuration.EquippedImageLeft, material);
            var maxDamage = GenerateMaxDamage(rarenessConfiguration);
            var minDamage = maxDamage.ToDictionary(pair => pair.Key, pair => pair.Value - rarenessConfiguration.MinMaxDamageDifference);
            var hitChance = RandomHelper.GetRandomValue(rarenessConfiguration.MinHitChance, rarenessConfiguration.MaxHitChance);
            var weightConfiguration = GetWeightConfiguration(material);
            var name = GenerateName(material);
            var description = GenerateDescription(rareness, material);
            var bonusesCount = RandomHelper.GetRandomValue(rarenessConfiguration.MinBonuses, rarenessConfiguration.MaxBonuses);
            var maxDurability = weightConfiguration.Durability;
            
            var itemConfig = new WeaponItemConfiguration
            {
                Name = name,
                Key = Guid.NewGuid().ToString(),
                Description = description,
                Rareness = rareness,
                Weight = weightConfiguration.Weight,
                MaxDamage = maxDamage,
                MinDamage = minDamage,
                HitChance = hitChance,
                InventoryImage = inventoryImage,
                WorldImage = worldImage,
                EquippedImageRight = equippedRightImage,
                EquippedImageLeft = equippedLeftImage,
                MaxDurability = maxDurability
            };
            bonusesGenerator.GenerateBonuses(itemConfig, bonusesCount);

            var durabilityPercent = RandomHelper.GetRandomValue(MinDurabilityPercent, MaxDurabilityPercent);
            var durability = Math.Min(itemConfig.MaxDurability, (int)Math.Round(itemConfig.MaxDurability * (durabilityPercent / 100d)));
            itemConfig.Durability = durability;

            return new WeaponItem(itemConfig);
        }

        private SymbolsImage GetMaterialColoredImage(string imageName, ItemMaterial material)
        {
            var image = imagesStorage.GetImage(imageName);
            return ItemRecolorHelper.RecolorItemImage(image, material);
        }

        private Dictionary<Element, int> GenerateMaxDamage(IWeaponRarenessConfiguration config)
        {
            return config.Damage.ToDictionary(pair => pair.Element,
                pair => RandomHelper.GetRandomValue(pair.MinValue, pair.MaxValue));
        }

        private IWeightConfiguration GetWeightConfiguration(ItemMaterial material)
        {
            var result = configuration.Weight.FirstOrDefault(config => config.Material == material);
            if (result == null)
                throw new ApplicationException($"No {baseName} weight configuration for material: {material}");

            return result;
        }

        private SymbolsImage MergeImages(params SymbolsImage[] images)
        {
            if (images.Length == 0)
                throw new ArgumentException("No images to merge.");

            var result = images[0];
            for (int index = 1; index < images.Length; index++)
            {
                result = SymbolsImage.Combine(result, images[index]);
            }

            return result;
        }

        private IWeaponRarenessConfiguration GetRarenessConfiguration(ItemRareness rareness)
        {
            var result = configuration.RarenessConfiguration.FirstOrDefault(config => config.Rareness == rareness);
            if (result == null)
                throw new ApplicationException($"No {baseName} rareness configuration for rareness: {rareness}");

            return result;
        }

        private SymbolsImage GenerateImage(ItemMaterial material)
        {
            var parts = configuration.Images.Sprites.OrderBy(sprite => sprite.Index)
                .Select(sprite => imagesStorage.GetImage(RandomHelper.GetRandomElement(sprite.Images)))
                .Select(image => ItemRecolorHelper.RecolorItemImage(image, material))
                .ToArray();
            return MergeImages(parts);
        }

        private string GenerateName(ItemMaterial material)
        {
            var materialPrefix = NameGenerationHelper.GetMaterialPrefix(material);
            return $"{materialPrefix} {baseName}";
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
            var textConfig = configurations.DescriptionConfiguration.MaterialDescription.FirstOrDefault(config => config.Material == material);
            if (textConfig == null)
                throw new ApplicationException($"Text configuration not found for weapon material: {material}");

            return RandomHelper.GetRandomElement(textConfig.Text);
        }

        private string GetRarenessDescription(ItemRareness rareness)
        {
            var textConfig = configurations.DescriptionConfiguration.RarenessDescription.FirstOrDefault(config => config.Rareness == rareness);
            if (textConfig == null)
                throw new ApplicationException($"Text configuration not found for weapon rareness: {rareness}");

            return RandomHelper.GetRandomElement(textConfig.Text);
        }
    }
}