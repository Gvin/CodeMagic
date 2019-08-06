using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.Implementations.Items.Weapon;
using CodeMagic.ItemsGeneration.Configuration.Weapon;
using CodeMagic.UI.Images;

namespace CodeMagic.ItemsGeneration.Implementations
{
    internal abstract class WeaponGeneratorBase : IWeaponGenerator
    {
        private readonly IImagesStorage imagesStorage;
        protected readonly string BaseName;

        protected WeaponGeneratorBase(string baseName, IImagesStorage imagesStorage)
        {
            this.imagesStorage = imagesStorage;
            BaseName = baseName;
        }

        public WeaponItem GenerateWeapon(ItemRareness rareness)
        {
            var rarenessConfiguration = GetRarenessConfiguration(rareness);
            var material = GetRandomElement(rarenessConfiguration.Materials);
            var image = GenerateImage(material);
            var maxDamage = RandomHelper.GetRandomValue(rarenessConfiguration.MinDamage, rarenessConfiguration.MaxDamage);
            var minDamage = Math.Max(0, maxDamage - rarenessConfiguration.MinMaxDamageDifference);
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

        protected string GetMaterialPrefix(ItemMaterial material)
        {
            switch (material)
            {
                case ItemMaterial.Wood:
                    return "Wooden";
                case ItemMaterial.Iron:
                    return "Iron";
                case ItemMaterial.Steel:
                    return "Steel";
                case ItemMaterial.Silver:
                    return "Silver";
                case ItemMaterial.ElvesMetal:
                    return "Elven";
                case ItemMaterial.DwarfsMetal:
                    return "Dwarf";
                case ItemMaterial.Mythril:
                    return "Mythril";
                default:
                    throw new ArgumentException($"Unknown material: {material}");
            }
        }

        protected string GetRarenessPrefix(ItemRareness rareness)
        {
            switch (rareness)
            {
                case ItemRareness.Trash:
                    return "Weak";
                case ItemRareness.Common:
                    return string.Empty;
                case ItemRareness.Uncommon:
                    return "Good";
                case ItemRareness.Rare:
                    return "Excellent";
                default:
                    throw new ArgumentException($"Unsupported rareness: {rareness}");
            }
        }

        protected abstract int GetWeight(ItemMaterial material);

        protected SymbolsImage GetRandomImage(string[] names)
        {
            var randomName = GetRandomElement(names);
            return imagesStorage.GetImage(randomName);
        }

        private T GetRandomElement<T>(T[] array)
        {
            if (array.Length == 0)
                throw new ArgumentException("Empty array.");

            return array[RandomHelper.GetRandomValue(0, array.Length - 1)];
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
            var rarenessPrefix = GetRarenessPrefix(rareness);
            var materialPrefix = GetMaterialPrefix(material);
            return $"{rarenessPrefix} {materialPrefix} {BaseName}";
        }

        protected abstract string[] GenerateDescription(ItemRareness rareness, ItemMaterial material);
    }
}