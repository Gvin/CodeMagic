using System;
using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.ItemsGeneration.Configuration.Weapon;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Head;
using CodeMagic.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.UI.Images;

namespace CodeMagic.ItemsGeneration.Implementations.Weapon
{
    internal class HeadWeaponGenerator : WeaponGeneratorBase
    {
        private readonly IHeadWeaponConfiguration configuration;

        public HeadWeaponGenerator(
            string baseName,
            string worldImageName,
            IHeadWeaponConfiguration configuration, 
            IWeaponConfiguration weaponConfiguration,
            BonusesGenerator bonusesGenerator,
            IImagesStorage imagesStorage) 
            : base(baseName, worldImageName, weaponConfiguration, bonusesGenerator, imagesStorage)
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
            var shaftImageInit = GetRandomImage(configuration.Images.ShaftImages);
            var headImageInit = GetRandomImage(configuration.Images.HeadImages);

            var handleImage = ItemRecolorHelper.RecolorItemImage(handleImageInit, material);
            var shaftImage = ItemRecolorHelper.RecolorItemImage(shaftImageInit, material);
            var headImage = ItemRecolorHelper.RecolorItemImage(headImageInit, material);

            return MergeImages(shaftImage, handleImage, headImage);
        }
    }
}