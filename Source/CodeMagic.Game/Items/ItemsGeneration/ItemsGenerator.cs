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

        ShieldItem GenerateShield(ItemRareness rareness);

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

        private readonly Dictionary<WeaponType, IWeaponGenerator> _weaponGenerators;
        private readonly ArmorGenerator _armorGenerator;
        private readonly SpellBookGenerator _spellBookGenerator;
        private readonly UsableItemsGenerator _usableItemsGenerator;
        private readonly ResourceItemsGenerator _resourceItemsGenerator;
        private readonly FoodItemsGenerator _foodItemsGenerator;
        private readonly ShieldGenerator _shieldGenerator;

        public ItemsGenerator(IItemGeneratorConfiguration configuration, IImagesStorage imagesStorage, IAncientSpellsProvider spellsProvider)
        {
            var bonusesGenerator = new BonusesGenerator(configuration.BonusesConfiguration);

            _weaponGenerators = new Dictionary<WeaponType, IWeaponGenerator>
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
            _armorGenerator = new ArmorGenerator(configuration.ArmorConfiguration, bonusesGenerator, imagesStorage);
            _shieldGenerator = new ShieldGenerator(configuration.ShieldsConfiguration, bonusesGenerator, imagesStorage);
            _spellBookGenerator = new SpellBookGenerator(configuration.SpellBooksConfiguration, bonusesGenerator, imagesStorage);
            _usableItemsGenerator = new UsableItemsGenerator(imagesStorage, spellsProvider);
            _resourceItemsGenerator = new ResourceItemsGenerator();
            _foodItemsGenerator = new FoodItemsGenerator(imagesStorage);
        }

        public WeaponItem GenerateWeapon(ItemRareness rareness)
        {
            if (GetIfRarenessExceedMax(rareness))
            {
                throw new ArgumentException("Item generator cannot generate epic items.");
            }

            var weaponType = GetRandomWeaponType();
            var generator = _weaponGenerators[weaponType];
            return generator.GenerateWeapon(rareness);
        }

        public ShieldItem GenerateShield(ItemRareness rareness)
        {
            if (GetIfRarenessExceedMax(rareness))
            {
                throw new ArgumentException("Item generator cannot generate epic items.");
            }

            return _shieldGenerator.GenerateShield(rareness);
        }

        public ArmorItem GenerateArmor(ItemRareness rareness, ArmorClass armorClass)
        {
            if (GetIfRarenessExceedMax(rareness))
            {
                throw new ArgumentException("Item generator cannot generate epic items.");
            }

            return _armorGenerator.GenerateArmor(rareness, armorClass);
        }

        public SpellBook GenerateSpellBook(ItemRareness rareness)
        {
            if (GetIfRarenessExceedMax(rareness))
            {
                throw new ArgumentException("Item generator cannot generate epic items.");
            }

            return _spellBookGenerator.GenerateSpellBook(rareness);
        }

        public IItem GenerateUsable(ItemRareness rareness)
        {
            if (GetIfRarenessExceedMax(rareness))
            {
                throw new ArgumentException("Item generator cannot generate epic items.");
            }

            return _usableItemsGenerator.GenerateUsableItem(rareness);
        }

        public IItem GenerateResource(ItemRareness rareness)
        {
            if (GetIfRarenessExceedMax(rareness))
            {
                throw new ArgumentException("Item generator cannot generate epic items.");
            }

            return _resourceItemsGenerator.GenerateResourceItem(rareness);
        }

        public IItem GenerateFood()
        {
            return _foodItemsGenerator.GenerateFood();
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
