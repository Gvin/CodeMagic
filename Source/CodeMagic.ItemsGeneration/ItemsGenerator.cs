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
                    new BladeWeaponGenerator("Sword", configuration.WeaponConfiguration.SwordsConfiguration, imagesStorage)
                },
                {
                    WeaponType.Dagger,
                    new BladeWeaponGenerator("Dagger", configuration.WeaponConfiguration.DaggersConfiguration, imagesStorage)
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
            return (WeaponType) RandomHelper.GetRandomValue(0, 1);
        }

        private enum WeaponType
        {
            Sword = 0,
            Dagger = 1
        }
    }
}
