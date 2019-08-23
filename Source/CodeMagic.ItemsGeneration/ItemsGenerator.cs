using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.ItemsGeneration.Configuration;
using CodeMagic.ItemsGeneration.Implementations;
using CodeMagic.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.ItemsGeneration.Implementations.Weapon;

namespace CodeMagic.ItemsGeneration
{
    public class ItemsGenerator : IItemsGenerator
    {
        private const string WorldImageNameSword = "ItemsOnGround_Weapon_Sword";
        private const string WorldImageNameAxe = "ItemsOnGround_Weapon_Axe";
        private const string WorldImageNameDagger = "ItemsOnGround_Weapon_Dagger";
        private const string WorldImageNameMace = "ItemsOnGround_Weapon_Mace";

        private readonly Dictionary<WeaponType, IWeaponGenerator> weaponGenerators;
        private readonly ArmorGenerator armorGenerator;
        private readonly SpellBookGenerator spellBookGenerator;
        private readonly PotionsGenerator potionsGenerator;
        private readonly ResourcesGenerator resourcesGenerator;

        public ItemsGenerator(IItemGeneratorConfiguration configuration, IImagesStorage imagesStorage)
        {
            var bonusesGenerator = new BonusesGenerator(configuration.BonusesConfiguration);

            weaponGenerators = new Dictionary<WeaponType, IWeaponGenerator>
            {
                {
                    WeaponType.Sword,
                    new BladeWeaponGenerator("Sword", 
                        WorldImageNameSword,
                        configuration.WeaponConfiguration.SwordsConfiguration, 
                        configuration.WeaponConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                },
                {
                    WeaponType.Dagger,
                    new BladeWeaponGenerator("Dagger", 
                        WorldImageNameDagger,
                        configuration.WeaponConfiguration.DaggersConfiguration,
                        configuration.WeaponConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                },
                {
                    WeaponType.Mace,
                    new HeadWeaponGenerator("Mace", 
                        WorldImageNameMace,
                        configuration.WeaponConfiguration.MacesConfiguration,
                        configuration.WeaponConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                },
                {
                    WeaponType.Axe,
                    new HeadWeaponGenerator("Axe",
                        WorldImageNameAxe,
                        configuration.WeaponConfiguration.AxesConfiguration,
                        configuration.WeaponConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                }
            };
            armorGenerator = new ArmorGenerator(configuration.ArmorConfiguration, bonusesGenerator, imagesStorage);
            spellBookGenerator = new SpellBookGenerator(configuration.SpellBooksConfiguration, bonusesGenerator, imagesStorage);
            potionsGenerator = new PotionsGenerator(imagesStorage);
            resourcesGenerator = new ResourcesGenerator();
        }

        public WeaponItem GenerateWeapon(ItemRareness rareness)
        {
            if (rareness == ItemRareness.Epic)
                throw new ArgumentException("Item generator cannot generate epic items.");

            var weaponType = GetRandomWeaponType();
            var generator = weaponGenerators[weaponType];
            return generator.GenerateWeapon(rareness);
        }

        public ArmorItem GenerateArmor(ItemRareness rareness, ArmorClass armorClass)
        {
            if (rareness == ItemRareness.Epic)
                throw new ArgumentException("Item generator cannot generate epic items.");

            return armorGenerator.GenerateArmor(rareness, armorClass);
        }

        public SpellBook GenerateSpellBook(ItemRareness rareness)
        {
            if (rareness == ItemRareness.Epic)
                throw new ArgumentException("Item generator cannot generate epic items.");

            return spellBookGenerator.GenerateSpellBook(rareness);
        }

        public IItem GeneratePotion(ItemRareness rareness)
        {
            if (rareness == ItemRareness.Epic)
                throw new ArgumentException("Item generator cannot generate epic items.");

            return potionsGenerator.GeneratePotion(rareness);
        }

        public IItem GenerateResource()
        {
            return resourcesGenerator.GenerateResource();
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
