using System;
using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.ItemsGeneration.Configuration.Weapon;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Blade;
using CodeMagic.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.UI.Images;

namespace CodeMagic.ItemsGeneration.Implementations.Weapon
{
    internal class BladeWeaponGenerator : WeaponGeneratorBase
    {
        private readonly IBladeWeaponConfiguration configuration;

        public BladeWeaponGenerator(
            string baseName,
            IBladeWeaponConfiguration configuration,
            IWeaponConfiguration weaponConfiguration,
            BonusesGenerator bonusesGenerator,
            IImagesStorage imagesStorage) 
            : base(baseName, weaponConfiguration, bonusesGenerator, imagesStorage)
        {
            this.configuration = configuration;
        }

        protected override int GetWeight(ItemMaterial material)
        {
            var result = configuration.Weight.FirstOrDefault(config => config.Material == material);
            if (result == null)
                throw new ApplicationException($"No {BaseName} weight configuration for material: {material}");

            return result.Weight;
        }

        protected override IWeaponRarenessConfiguration GetRarenessConfiguration(ItemRareness rareness)
        {
            var result = configuration.RarenessConfiguration.FirstOrDefault(config => config.Rareness == rareness);
            if (result == null)
                throw new ApplicationException($"No {BaseName} rareness configuration for rareness: {rareness}");

            return result;
        }

        protected override SymbolsImage GenerateImage(ItemMaterial material)
        {
            var handleImageInit = GetRandomImage(configuration.Images.HandleImages);
            var guardImageInit = GetRandomImage(configuration.Images.GuardImages);
            var bladeImageInit = GetRandomImage(configuration.Images.BladeImages);

            var handleImage = ItemRecolorHelper.RecolorItemImage(handleImageInit, material);
            var guardImage = ItemRecolorHelper.RecolorItemImage(guardImageInit, material);
            var bladeImage = ItemRecolorHelper.RecolorItemImage(bladeImageInit, material);

            return MergeImages(bladeImage, handleImage, guardImage);
        }
    }
}