using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Objects.Creatures.Loot
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

        private readonly Chance<int>[] resourceCountSettings;

        public ChancesLootGenerator(ILootConfiguration lootConfiguration)
        {
            weaponCountSettings = lootConfiguration.Weapon?.Count?.Select(c => new Chance<int>(c.Chance, c.Value)).ToArray();
            weaponRarenessSettings = lootConfiguration.Weapon?.Rareness?.Select(c => new Chance<ItemRareness>(c.Chance, c.Value)).ToArray();

            armorCountSettings = lootConfiguration.Armor?.Count?.Select(c => new Chance<int>(c.Chance, c.Value)).ToArray();
            armorRarenessSettings = lootConfiguration.Armor?.Rareness?.Select(c => new Chance<ItemRareness>(c.Chance, c.Value)).ToArray();
            armorClassSettings = lootConfiguration.Armor?.Class?.Select(c => new Chance<ArmorClass>(c.Chance, c.Value)).ToArray();

            spellBookCountSettings = lootConfiguration.SpellBook?.Count?.Select(c => new Chance<int>(c.Chance, c.Value)).ToArray();
            spellBookRarenessSettings = lootConfiguration.SpellBook?.Rareness?.Select(c => new Chance<ItemRareness>(c.Chance, c.Value)).ToArray();

            usableCountSettings = lootConfiguration.Usable?.Count?.Select(c => new Chance<int>(c.Chance, c.Value)).ToArray();
            usableRarenessSettings = lootConfiguration.Usable?.Rareness?.Select(c => new Chance<ItemRareness>(c.Chance, c.Value)).ToArray();

            resourceCountSettings = lootConfiguration.Resource?.Count?.Select(c => new Chance<int>(c.Chance, c.Value)).ToArray();
        }

        public IItem[] GenerateLoot()
        {
            var generator = Injector.Current.Create<IItemsGenerator>();
            var result = new List<IItem>();

            if (weaponCountSettings != null)
            {
                var items = GenerateItems(
                    weaponCountSettings, 
                    weaponRarenessSettings, 
                    rareness => generator.GenerateWeapon(rareness));
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
                    rareness => generator.GenerateSpellBook(rareness));
                result.AddRange(items);
            }

            if (usableCountSettings != null)
            {
                var items = GenerateItems(
                    usableCountSettings, 
                    usableRarenessSettings, 
                    rareness => generator.GenerateUsable(rareness));
                result.AddRange(items);
            }

            if (resourceCountSettings != null)
            {
                var count = GenerateValue(resourceCountSettings);
                for (int counter = 0; counter < count; counter++)
                {
                    var resource = generator.GenerateResource();
                    if (resource != null)
                    {
                        result.Add(resource);
                    }
                }
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