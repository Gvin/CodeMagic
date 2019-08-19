using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Chance<int>[] potionCountSettings;
        private readonly Chance<ItemRareness>[] potionRarenessSettings;

        public ChancesLootGenerator(
            Chance<int>[] weaponCountSettings = null, 
            Chance<ItemRareness>[] weaponRarenessSettings = null, 
            Chance<int>[] armorCountSettings = null, 
            Chance<ItemRareness>[] armorRarenessSettings = null, 
            Chance<ArmorClass>[] armorClassSettings = null, 
            Chance<int>[] spellBookCountSettings = null, 
            Chance<ItemRareness>[] spellBookRarenessSettings = null, 
            Chance<int>[] potionCountSettings = null, 
            Chance<ItemRareness>[] potionRarenessSettings = null)
        {
            this.weaponCountSettings = weaponCountSettings;
            this.weaponRarenessSettings = weaponRarenessSettings;
            this.armorCountSettings = armorCountSettings;
            this.armorRarenessSettings = armorRarenessSettings;
            this.armorClassSettings = armorClassSettings;
            this.spellBookCountSettings = spellBookCountSettings;
            this.spellBookRarenessSettings = spellBookRarenessSettings;
            this.potionCountSettings = potionCountSettings;
            this.potionRarenessSettings = potionRarenessSettings;
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

            if (potionCountSettings != null)
            {
                var items = GenerateItems(
                    potionCountSettings, 
                    potionRarenessSettings, 
                    rareness => generator.GeneratePotion(rareness));
                result.AddRange(items);
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