using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Implementations;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Weapon;

namespace CodeMagic.Game.Items.ItemsGeneration
{
    public interface IItemsGenerator
    {
        WeaponItem GenerateWeapon(ItemRareness rareness);

        ArmorItem GenerateArmor(ItemRareness rareness, ArmorClass armorClass);

        SpellBook GenerateSpellBook(ItemRareness rareness);

        IItem GenerateUsable(ItemRareness rareness);

        IItem GenerateResource(ItemRareness rareness);

        IItem GenerateFood();
    }

    public class ItemsGenerator : IItemsGenerator
    {
        private const string WorldImageNameSword = "ItemsOnGround_Weapon_Sword";
        private const string WorldImageNameAxe = "ItemsOnGround_Weapon_Axe";
        private const string WorldImageNameDagger = "ItemsOnGround_Weapon_Dagger";
        private const string WorldImageNameMace = "ItemsOnGround_Weapon_Mace";
        private const string WorldImageNameStaff = "ItemsOnGround_Weapon_Staff";

        private readonly Dictionary<WeaponType, IWeaponGenerator> weaponGenerators;
        private readonly ArmorGenerator armorGenerator;
        private readonly SpellBookGenerator spellBookGenerator;
        private readonly UsableItemsGenerator usableItemsGenerator;
        private readonly ResourceItemsGenerator resourceItemsGenerator;
        private readonly FoodItemsGenerator foodItemsGenerator;

        public ItemsGenerator(IItemGeneratorConfiguration configuration, IImagesStorage imagesStorage, IAncientSpellsProvider spellsProvider)
        {
            var bonusesGenerator = new BonusesGenerator(configuration.BonusesConfiguration);

            weaponGenerators = new Dictionary<WeaponType, IWeaponGenerator>
            {
                {
                    WeaponType.Sword,
                    new WeaponGenerator("Sword", 
                        WorldImageNameSword,
                        configuration.WeaponsConfiguration.SwordsConfiguration, 
                        configuration.WeaponsConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                },
                {
                    WeaponType.Dagger,
                    new WeaponGenerator("Dagger", 
                        WorldImageNameDagger,
                        configuration.WeaponsConfiguration.DaggersConfiguration,
                        configuration.WeaponsConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                },
                {
                    WeaponType.Mace,
                    new WeaponGenerator("Mace", 
                        WorldImageNameMace,
                        configuration.WeaponsConfiguration.MacesConfiguration,
                        configuration.WeaponsConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                },
                {
                    WeaponType.Axe,
                    new WeaponGenerator("Axe",
                        WorldImageNameAxe,
                        configuration.WeaponsConfiguration.AxesConfiguration,
                        configuration.WeaponsConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                },
                {
                    WeaponType.Staff,
                    new WeaponGenerator("Staff",
                        WorldImageNameStaff,
                        configuration.WeaponsConfiguration.StaffsConfiguration,
                        configuration.WeaponsConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                }
            };
            armorGenerator = new ArmorGenerator(configuration.ArmorConfiguration, bonusesGenerator, imagesStorage);
            spellBookGenerator = new SpellBookGenerator(configuration.SpellBooksConfiguration, bonusesGenerator, imagesStorage);
            usableItemsGenerator = new UsableItemsGenerator(imagesStorage, spellsProvider);
            resourceItemsGenerator = new ResourceItemsGenerator();
            foodItemsGenerator = new FoodItemsGenerator(imagesStorage);
        }

        public WeaponItem GenerateWeapon(ItemRareness rareness)
        {
            if (GetIfRarenessExceedMax(rareness))
                throw new ArgumentException("Item generator cannot generate epic items.");

            var weaponType = GetRandomWeaponType();
            var generator = weaponGenerators[weaponType];
            return generator.GenerateWeapon(rareness);
        }

        public ArmorItem GenerateArmor(ItemRareness rareness, ArmorClass armorClass)
        {
            if (GetIfRarenessExceedMax(rareness))
                throw new ArgumentException("Item generator cannot generate epic items.");

            return armorGenerator.GenerateArmor(rareness, armorClass);
        }

        public SpellBook GenerateSpellBook(ItemRareness rareness)
        {
            if (GetIfRarenessExceedMax(rareness))
                throw new ArgumentException("Item generator cannot generate epic items.");

            return spellBookGenerator.GenerateSpellBook(rareness);
        }

        public IItem GenerateUsable(ItemRareness rareness)
        {
            if (GetIfRarenessExceedMax(rareness))
                throw new ArgumentException("Item generator cannot generate epic items.");

            return usableItemsGenerator.GenerateUsableItem(rareness);
        }

        public IItem GenerateResource(ItemRareness rareness)
        {
            if (GetIfRarenessExceedMax(rareness))
                throw new ArgumentException("Item generator cannot generate epic items.");

            return resourceItemsGenerator.GenerateResourceItem(rareness);
        }

        public IItem GenerateFood()
        {
            return foodItemsGenerator.GenerateFood();
        }

        private static bool GetIfRarenessExceedMax(ItemRareness rareness)
        {
            return rareness == ItemRareness.Epic;
        }

        private WeaponType GetRandomWeaponType()
        {
            return RandomHelper.GetRandomElement(Enum.GetValues(typeof(WeaponType)).Cast<WeaponType>().ToArray());
        }

        private enum WeaponType
        {
            Sword,
            Dagger,
            Mace,
            Axe,
            Staff
        }
    }
}
