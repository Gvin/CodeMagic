using System;
using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.ItemsGeneration.Configuration.Weapon;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Head;
using CodeMagic.UI.Images;

namespace CodeMagic.ItemsGeneration.Implementations
{
    internal class HeadWeaponGenerator : WeaponGeneratorBase
    {
        private readonly IHeadWeaponConfiguration configuration;

        public HeadWeaponGenerator(
            string baseName, 
            IHeadWeaponConfiguration configuration, 
            IWeaponConfiguration weaponConfiguration, 
            IImagesStorage imagesStorage) 
            : base(baseName, weaponConfiguration, imagesStorage)
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

            var handleImage = ItemRecolorHelper.RecolorImage(handleImageInit, material);
            var shaftImage = ItemRecolorHelper.RecolorImage(shaftImageInit, material);
            var headImage = ItemRecolorHelper.RecolorImage(headImageInit, material);

            return MergeImages(shaftImage, handleImage, headImage);
        }
    }
}