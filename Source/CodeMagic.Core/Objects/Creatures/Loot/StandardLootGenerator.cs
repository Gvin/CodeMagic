using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Objects.Creatures.Loot
{
    public class StandardLootGenerator : ILootGenerator
    {
        private readonly int weaponsCountMin;
        private readonly int weaponsCountMax;

        private readonly int armorCountMin;
        private readonly int armorCountMax;

        private readonly int spellBooksCountMin;
        private readonly int spellBooksCountMax;

        private readonly int potionsCountMin;
        private readonly int potionsCountMax;

        private readonly ItemRareness rarenessMin;
        private readonly ItemRareness rarenessMax;

        public StandardLootGenerator(ItemRareness rarenessMin, ItemRareness rarenessMax,
            int weaponsCountMin = 0, int weaponsCountMax = 0,
            int armorCountMin = 0, int armorCountMax = 0,
            int spellBooksCountMin = 0, int spellBooksCountMax = 0,
            int potionsCountMin = 0, int potionsCountMax = 0)
        {
            this.rarenessMin = rarenessMin;
            this.rarenessMax = rarenessMax;

            this.weaponsCountMin = weaponsCountMin;
            this.weaponsCountMax = weaponsCountMax;

            this.armorCountMin = armorCountMin;
            this.armorCountMax = armorCountMax;

            this.spellBooksCountMin = spellBooksCountMin;
            this.spellBooksCountMax = spellBooksCountMax;

            this.potionsCountMin = potionsCountMin;
            this.potionsCountMax = potionsCountMax;
        }


        public IItem[] GenerateLoot()
        {
            var result = new List<IItem>();

            var generator = Injector.Current.Create<IItemsGenerator>();

            var weaponsCount = GetRandomValue(weaponsCountMin, weaponsCountMax);
            var weapons = GenerateItems(weaponsCount, rareness => generator.GenerateWeapon(rareness));
            result.AddRange(weapons);

            var armorCount = GetRandomValue(armorCountMin, armorCountMax);
            var armor = GenerateItems(armorCount, rareness => generator.GenerateArmor(rareness));
            result.AddRange(armor);

            var spellBooksCount = GetRandomValue(spellBooksCountMin, spellBooksCountMax);
            var spellBooks = GenerateItems(spellBooksCount, rareness => generator.GenerateSpellBook(rareness));
            result.AddRange(spellBooks);

            var potionsCount = GetRandomValue(potionsCountMin, potionsCountMax);
            var potions = GenerateItems(potionsCount, rareness => generator.GeneratePotion(rareness));
            result.AddRange(potions);

            return result.ToArray();
        }

        private IItem[] GenerateItems(int count, Func<ItemRareness, IItem> generator)
        {
            var result = new List<IItem>(count);
            for (int counter = 0; counter < count; counter++)
            {
                var rareness = GetRandomRareness();
                var item = generator(rareness);
                result.Add(item);
            }

            return result.Where(item => item != null).ToArray();
        }

        private ItemRareness GetRandomRareness()
        {
            return (ItemRareness) GetRandomValue((int) rarenessMin, (int) rarenessMax);
        }

        private int GetRandomValue(int min, int max)
        {
            if (min == max)
                return min;

            return RandomHelper.GetRandomValue(min, max);
        }
    }
}