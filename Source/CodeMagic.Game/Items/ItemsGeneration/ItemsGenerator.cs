using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Implementations;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Weapon;
using CodeMagic.Game.Items.Usable.Food;

namespace CodeMagic.Game.Items.ItemsGeneration
{
    public interface IItemsGenerator
    {
        WeaponItem GenerateWeapon(ItemRareness rareness);

        ArmorItem GenerateArmor(ItemRareness rareness, ArmorClass armorClass);

        SpellBook GenerateSpellBook(ItemRareness rareness);

        IItem GenerateUsable(ItemRareness rareness);

        IItem GenerateFood();

        void Reset();
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
        }

        public WeaponItem GenerateWeapon(ItemRareness rareness)
        {
            if (rareness == ItemRareness.Epic)
                throw new ArgumentException("Item generator cannot generate epic items.");

            var weaponType = GetRandomWeaponType();
            var generator = weaponGenerators[weaponType];
            return generator.GenerateWeapon(rareness);
        }

        public void Reset()
        {
            usableItemsGenerator.Reset();
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

        public IItem GenerateUsable(ItemRareness rareness)
        {
            if (rareness == ItemRareness.Epic)
                throw new ArgumentException("Item generator cannot generate epic items.");

            return usableItemsGenerator.GenerateUsableItem(rareness);
        }

        public IItem GenerateRandomItem(ItemRareness rareness)
        {
            var generatorType = RandomHelper.GetRandomValue(1, 5);
            switch (generatorType)
            {
                case 1:
                    return GenerateWeapon(rareness);
                case 2:
                    return GenerateArmor(rareness, RandomHelper.GetRandomEnumValue<ArmorClass>());
                case 3:
                    return GenerateSpellBook(rareness);
                case 4:
                    return GenerateUsable(rareness);
                case 5:
                    return GenerateFood();
                default:
                    throw new ApplicationException($"Unknown generator type: {generatorType}.");
            }
        }

        public IItem GenerateFood()
        {
            return new Apple();
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
