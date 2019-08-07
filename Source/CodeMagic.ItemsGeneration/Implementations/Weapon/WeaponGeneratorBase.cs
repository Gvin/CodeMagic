using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.Implementations.Items.Weapon;
using CodeMagic.ItemsGeneration.Configuration.Weapon;
using CodeMagic.UI.Images;

namespace CodeMagic.ItemsGeneration.Implementations.Weapon
{
    internal abstract class WeaponGeneratorBase : IWeaponGenerator
    {
        private readonly IImagesStorage imagesStorage;
        protected readonly string BaseName;
        private readonly IWeaponConfiguration configuration;

        protected WeaponGeneratorBase(string baseName, IWeaponConfiguration configuration, IImagesStorage imagesStorage)
        {
            this.configuration = configuration;
            this.imagesStorage = imagesStorage;
            BaseName = baseName;
        }

        public WeaponItem GenerateWeapon(ItemRareness rareness)
        {
            var rarenessConfiguration = GetRarenessConfiguration(rareness);
            var material = RandomHelper.GetRandomElement(rarenessConfiguration.Materials);
            var image = GenerateImage(material);
            var maxDamage = GenerateMaxDamage(rarenessConfiguration);
            var minDamage = maxDamage.ToDictionary(pair => pair.Key, pair => pair.Value - rarenessConfiguration.MinMaxDamageDifference);
            var hitChance = RandomHelper.GetRandomValue(rarenessConfiguration.MinHitChance, rarenessConfiguration.MaxHitChance);
            var weight = GetWeight(material);
            var name = GenerateName(rareness, material);
            var description = GenerateDescription(rareness, material);

            return new WeaponItemImpl(new WeaponItemImplConfiguration
            {
                Name = name,
                Key = Guid.NewGuid().ToString(),
                Description = description,
                Rareness = rareness,
                Weight = weight,
                MaxDamage = maxDamage,
                MinDamage = minDamage,
                HitChance = hitChance,
                Image = image
            });
        }

        private Dictionary<Element, int> GenerateMaxDamage(IWeaponRarenessConfiguration config)
        {
            return config.Damage.ToDictionary(pair => pair.Element,
                pair => RandomHelper.GetRandomValue(pair.MinValue, pair.MaxValue));
        }

        protected abstract int GetWeight(ItemMaterial material);

        protected SymbolsImage GetRandomImage(string[] names)
        {
            var randomName = RandomHelper.GetRandomElement(names);
            return imagesStorage.GetImage(randomName);
        }

        protected SymbolsImage MergeImages(params SymbolsImage[] images)
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

        protected abstract IWeaponRarenessConfiguration GetRarenessConfiguration(ItemRareness rareness);

        protected abstract SymbolsImage GenerateImage(ItemMaterial material);

        private string GenerateName(ItemRareness rareness, ItemMaterial material)
        {
            var rarenessPrefix = NameGenerationHelper.GetRarenessPrefix(rareness);
            var materialPrefix = NameGenerationHelper.GetMaterialPrefix(material);
            if (string.IsNullOrEmpty(rarenessPrefix))
            {
                return $"{materialPrefix} {BaseName}";
            }
            return $"{rarenessPrefix} {materialPrefix} {BaseName}";
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
                throw new ApplicationException($"Text configuration not found for weapon material: {material}");

            return RandomHelper.GetRandomElement(textConfig.Text);
        }

        private string GetRarenessDescription(ItemRareness rareness)
        {
            var textConfig = configuration.DescriptionConfiguration.RarenessDescription.FirstOrDefault(config => config.Rareness == rareness);
            if (textConfig == null)
                throw new ApplicationException($"Text configuration not found for weapon rareness: {rareness}");

            return RandomHelper.GetRandomElement(textConfig.Text);
        }
    }
}