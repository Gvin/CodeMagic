using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.ItemsGeneration.Configuration;
using CodeMagic.ItemsGeneration.Implementations;

namespace CodeMagic.ItemsGeneration
{
    public class ItemsGenerator : IItemsGenerator
    {
        private readonly Dictionary<WeaponType, IWeaponGenerator> weaponGenerators;

        public ItemsGenerator(IItemGeneratorConfiguration configuration, IImagesStorage imagesStorage)
        {
            weaponGenerators = new Dictionary<WeaponType, IWeaponGenerator>
            {
                {
                    WeaponType.Sword,
                    new BladeWeaponGenerator("Sword", 
                        configuration.WeaponConfiguration.SwordsConfiguration, 
                        configuration.WeaponConfiguration, 
                        imagesStorage)
                },
                {
                    WeaponType.Dagger,
                    new BladeWeaponGenerator("Dagger", 
                        configuration.WeaponConfiguration.DaggersConfiguration,
                        configuration.WeaponConfiguration,
                        imagesStorage)
                },
                {
                    WeaponType.Mace,
                    new HeadWeaponGenerator("Mace", 
                        configuration.WeaponConfiguration.MacesConfiguration,
                        configuration.WeaponConfiguration,
                        imagesStorage)
                },
                {
                    WeaponType.Axe,
                    new HeadWeaponGenerator("Axe",
                        configuration.WeaponConfiguration.AxesConfiguration,
                        configuration.WeaponConfiguration,
                        imagesStorage)
                }
            };
        }

        public WeaponItem GenerateWeapon(ItemRareness rareness)
        {
            if (rareness == ItemRareness.Epic)
                throw new ArgumentException("Item generator cannot generate epic items.");

            var weaponType = GetRandomWeaponType();
            var generator = weaponGenerators[weaponType];
            return generator.GenerateWeapon(rareness);
        }

        private WeaponType GetRandomWeaponType()
        {
            return (WeaponType) RandomHelper.GetRandomValue(0, 3);
        }

        private enum WeaponType
        {
            Sword = 0,
            Dagger = 1,
            Mace = 2,
            Axe = 3
        }
    }
}
