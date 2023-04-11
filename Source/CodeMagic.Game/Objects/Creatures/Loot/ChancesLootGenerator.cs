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
        private readonly Chance<int>[] _weaponCountSettings;
        private readonly Chance<ItemRareness>[] _weaponRarenessSettings;

        private readonly Chance<int>[] _armorCountSettings;
        private readonly Chance<ItemRareness>[] _armorRarenessSettings;
        private readonly Chance<ArmorClass>[] _armorClassSettings;

        private readonly Chance<int>[] _spellBookCountSettings;
        private readonly Chance<ItemRareness>[] _spellBookRarenessSettings;

        private readonly Chance<int>[] _usableCountSettings;
        private readonly Chance<ItemRareness>[] _usableRarenessSettings;

        private readonly Chance<int>[] _resourcesCountSettings;
        private readonly Chance<ItemRareness>[] _resourcesRarenessSettings;

        private readonly Chance<int>[] _shieldCountSettings;
        private readonly Chance<ItemRareness>[] _shieldRarenessSettings;

        private readonly Chance<int>[] _foodCountSettings;

        public ChancesLootGenerator(ILootConfiguration lootConfiguration)
        {
            _weaponCountSettings = GetChanceConfiguration(lootConfiguration.Weapon?.Count);
            _weaponRarenessSettings = GetChanceConfiguration(lootConfiguration.Weapon?.Rareness);

            _armorCountSettings = GetChanceConfiguration(lootConfiguration.Armor?.Count);
            _armorRarenessSettings = GetChanceConfiguration(lootConfiguration.Armor?.Rareness);
            _armorClassSettings = GetChanceConfiguration(lootConfiguration.Armor?.Class);

            _spellBookCountSettings = GetChanceConfiguration(lootConfiguration.SpellBook?.Count);
            _spellBookRarenessSettings = GetChanceConfiguration(lootConfiguration.SpellBook?.Rareness);

            _usableCountSettings = GetChanceConfiguration(lootConfiguration.Usable?.Count);
            _usableRarenessSettings = GetChanceConfiguration(lootConfiguration.Usable?.Rareness);

            _resourcesCountSettings = GetChanceConfiguration(lootConfiguration.Resource?.Count);
            _resourcesRarenessSettings = GetChanceConfiguration(lootConfiguration.Resource?.Rareness);

            _shieldCountSettings = GetChanceConfiguration(lootConfiguration.Shield?.Count);
            _shieldRarenessSettings = GetChanceConfiguration(lootConfiguration.Shield?.Rareness);

            _foodCountSettings = GetChanceConfiguration(lootConfiguration.Food?.Count);
        }

        public IItem[] GenerateLoot()
        {
            var generator = ItemsGeneratorManager.Generator;
            var result = new List<IItem>();

            if (_weaponCountSettings != null)
            {
                var items = GenerateItems(
                    _weaponCountSettings, 
                    _weaponRarenessSettings, 
                    generator.GenerateWeapon);
                result.AddRange(items);
            }

            if (_armorCountSettings != null)
            {
                var items = GenerateItems(
                    _armorCountSettings, 
                    _armorRarenessSettings, 
                    rareness => GenerateArmor(generator, rareness, _armorClassSettings));
                result.AddRange(items);
            }

            if (_spellBookCountSettings != null)
            {
                var items = GenerateItems(
                    _spellBookCountSettings, 
                    _spellBookRarenessSettings, 
                    generator.GenerateSpellBook);
                result.AddRange(items);
            }

            if (_usableCountSettings != null)
            {
                var items = GenerateItems(
                    _usableCountSettings, 
                    _usableRarenessSettings, 
                    generator.GenerateUsable);
                result.AddRange(items);
            }

            if (_resourcesCountSettings != null)
            {
                var items = GenerateItems(
                    _resourcesCountSettings,
                    _resourcesRarenessSettings,
                    generator.GenerateResource);
                result.AddRange(items);
            }

            if (_shieldCountSettings != null)
            {
                var items = GenerateItems(
                    _shieldCountSettings,
                    _shieldRarenessSettings,
                    generator.GenerateShield);
                result.AddRange(items);
            }

            if (_foodCountSettings != null)
            {
                var items = GenerateFood(generator);
                result.AddRange(items);
            }

            return result.ToArray();
        }

        private IItem[] GenerateFood(IItemsGenerator generator)
        {
            var count = GenerateValue(_foodCountSettings);
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

        private sealed class Chance<T>
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
}
