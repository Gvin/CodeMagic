using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.ItemsGeneration;

namespace CodeMagic.Game.Objects.Creatures.Loot
{
    public class ChancesLootGenerator : ILootGenerator
    {
        private readonly Chance<int>[] weaponCountSettings;
        private readonly Chance<ItemRareness>[] weaponRarenessSettings;

        private readonly Chance<int>[] armorCountSettings;
        private readonly Chance<ItemRareness>[] armorRarenessSettings;
        private readonly Chance<ArmorClass>[] armorClassSettings;

        private readonly Chance<int>[] spellBookCountSettings;
        private readonly Chance<ItemRareness>[] spellBookRarenessSettings;

        private readonly Chance<int>[] usableCountSettings;
        private readonly Chance<ItemRareness>[] usableRarenessSettings;

        private readonly Chance<int>[] resourcesCountSettings;
        private readonly Chance<ItemRareness>[] resourcesRarenessSettings;

        private readonly Chance<int>[] shieldCountSettings;
        private readonly Chance<ItemRareness>[] shieldRarenessSettings;

        private readonly Chance<int>[] foodCountSettings;

        public ChancesLootGenerator(ILootConfiguration lootConfiguration)
        {
            weaponCountSettings = GetChanceConfiguration(lootConfiguration.Weapon?.Count);
            weaponRarenessSettings = GetChanceConfiguration(lootConfiguration.Weapon?.Rareness);

            armorCountSettings = GetChanceConfiguration(lootConfiguration.Armor?.Count);
            armorRarenessSettings = GetChanceConfiguration(lootConfiguration.Armor?.Rareness);
            armorClassSettings = GetChanceConfiguration(lootConfiguration.Armor?.Class);

            spellBookCountSettings = GetChanceConfiguration(lootConfiguration.SpellBook?.Count);
            spellBookRarenessSettings = GetChanceConfiguration(lootConfiguration.SpellBook?.Rareness);

            usableCountSettings = GetChanceConfiguration(lootConfiguration.Usable?.Count);
            usableRarenessSettings = GetChanceConfiguration(lootConfiguration.Usable?.Rareness);

            resourcesCountSettings = GetChanceConfiguration(lootConfiguration.Resource?.Count);
            resourcesRarenessSettings = GetChanceConfiguration(lootConfiguration.Resource?.Rareness);

            shieldCountSettings = GetChanceConfiguration(lootConfiguration.Shield?.Count);
            shieldRarenessSettings = GetChanceConfiguration(lootConfiguration.Shield?.Rareness);

            foodCountSettings = GetChanceConfiguration(lootConfiguration.Food?.Count);
        }

        public IItem[] GenerateLoot()
        {
            var generator = ItemsGeneratorManager.Generator;
            var result = new List<IItem>();

            if (weaponCountSettings != null)
            {
                var items = GenerateItems(
                    weaponCountSettings, 
                    weaponRarenessSettings, 
                    generator.GenerateWeapon);
                result.AddRange(items);
            }

            if (armorCountSettings != null)
            {
                var items = GenerateItems(
                    armorCountSettings, 
                    armorRarenessSettings, 
                    rareness => GenerateArmor(generator, rareness, armorClassSettings));
                result.AddRange(items);
            }

            if (spellBookCountSettings != null)
            {
                var items = GenerateItems(
                    spellBookCountSettings, 
                    spellBookRarenessSettings, 
                    generator.GenerateSpellBook);
                result.AddRange(items);
            }

            if (usableCountSettings != null)
            {
                var items = GenerateItems(
                    usableCountSettings, 
                    usableRarenessSettings, 
                    generator.GenerateUsable);
                result.AddRange(items);
            }

            if (resourcesCountSettings != null)
            {
                var items = GenerateItems(
                    resourcesCountSettings,
                    resourcesRarenessSettings,
                    generator.GenerateResource);
                result.AddRange(items);
            }

            if (shieldCountSettings != null)
            {
                var items = GenerateItems(
                    shieldCountSettings,
                    shieldRarenessSettings,
                    generator.GenerateShield);
                result.AddRange(items);
            }

            if (foodCountSettings != null)
            {
                var items = GenerateFood(generator);
                result.AddRange(items);
            }

            return result.ToArray();
        }

        private IItem[] GenerateFood(IItemsGenerator generator)
        {
            var count = GenerateValue(foodCountSettings);
            var result = new List<IItem>();
            for (int counter = 0; counter < count; counter++)
            {
                result.Add(generator.GenerateFood());
            }

            return result.ToArray();
        }

        private IItem GenerateArmor(IItemsGenerator generator, ItemRareness rareness, Chance<ArmorClass>[] classSettings)
        {
            var armorClass = GenerateValue(classSettings);
            return generator.GenerateArmor(rareness, armorClass);
        }

        private IItem[] GenerateItems(Chance<int>[] countSettings, Chance<ItemRareness>[] rarenessSettings, Func<ItemRareness, IItem> generateMethod)
        {
            var count = GenerateValue(countSettings);
            var result = new List<IItem>();

            for (var counter = 0; counter < count; counter++)
            {
                var rareness = GenerateValue(rarenessSettings);
                var item = generateMethod(rareness);
                if (item != null)
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        private T GenerateValue<T>(Chance<T>[] chances)
        {
            var orderedChances = chances.OrderBy(chance => chance.ChanceValue).ToArray();

            var value = RandomHelper.GetRandomValue(1, 100);

            var startValue = 0;
            for (int index = 0; index < orderedChances.Length; index++)
            {
                var chance = orderedChances[index];

                if (value > startValue && value <= chance.ChanceValue + startValue)
                {
                    return chance.Setting;
                }

                startValue += chance.ChanceValue;
            }

            throw new ApplicationException("Cannot resolve chance.");
        }

        private static Chance<T>[] GetChanceConfiguration<T>(IChanceConfiguration<T>[] configuration)
        {
            return configuration?.Select(c => new Chance<T>(c.Chance, c.Value)).ToArray();
        }
    }

    public class Chance<T>
    {
        public Chance(int chanceValue, T setting)
        {
            ChanceValue = chanceValue;
            Setting = setting;
        }

        public int ChanceValue { get; }

        public T Setting { get; }
    }
}