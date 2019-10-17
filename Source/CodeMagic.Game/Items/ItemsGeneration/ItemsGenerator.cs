using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Implementations;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Tool;
using CodeMagic.Game.Items.ItemsGeneration.Implementations.Weapon;

namespace CodeMagic.Game.Items.ItemsGeneration
{
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
        private readonly ResourcesGenerator resourcesGenerator;
        private readonly LumberjackAxeGenerator lumberjackAxeGenerator;
        private readonly PickaxeGenerator pickaxeGenerator;

        public ItemsGenerator(IItemGeneratorConfiguration configuration, IImagesStorage imagesStorage, IAncientSpellsProvider spellsProvider)
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
                },
                {
                    WeaponType.Staff,
                    new HeadWeaponGenerator("Staff",
                        WorldImageNameStaff,
                        configuration.WeaponConfiguration.StaffsConfiguration,
                        configuration.WeaponConfiguration,
                        bonusesGenerator,
                        imagesStorage)
                }
            };
            armorGenerator = new ArmorGenerator(configuration.ArmorConfiguration, bonusesGenerator, imagesStorage);
            spellBookGenerator = new SpellBookGenerator(configuration.SpellBooksConfiguration, bonusesGenerator, imagesStorage);
            usableItemsGenerator = new UsableItemsGenerator(imagesStorage, spellsProvider);
            resourcesGenerator = new ResourcesGenerator();
            lumberjackAxeGenerator = new LumberjackAxeGenerator(configuration.ToolsConfiguration.LumberjackAxe, imagesStorage);
            pickaxeGenerator = new PickaxeGenerator(configuration.ToolsConfiguration.Pickaxe, imagesStorage);
        }

        public IItem GeneratePickaxe(ItemRareness rareness)
        {
            return pickaxeGenerator.GenerateTool(rareness);
        }

        public IItem GenerateLumberjackAxe(ItemRareness rareness)
        {
            return lumberjackAxeGenerator.GenerateTool(rareness);
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

        public IItem GenerateUsable(ItemRareness rareness)
        {
            if (rareness == ItemRareness.Epic)
                throw new ArgumentException("Item generator cannot generate epic items.");

            while (true)
            {
                var result = usableItemsGenerator.GenerateUsableItem(rareness);
                if (result != null)
                    return result;
            }
        }

        public IItem GenerateResource()
        {
            return resourcesGenerator.GenerateResource();
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
