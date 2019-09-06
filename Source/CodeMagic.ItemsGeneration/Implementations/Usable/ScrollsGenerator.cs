using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Spells;
using CodeMagic.Implementations.Items.Usable;

namespace CodeMagic.ItemsGeneration.Implementations.Usable
{
    public class ScrollsGenerator : IUsableItemTypeGenerator
    {
        private const int UncommonMinDamage = 15;
        private const int UncommonMaxDamage = 30;

        private const int RareMinDamage = 25;
        private const int RareMaxDamage = 50;

        private readonly IAncientSpellsProvider spellsProvider;
        private readonly Dictionary<BookSpell, string> spellNamesCache;

        public ScrollsGenerator(IAncientSpellsProvider spellsProvider)
        {
            this.spellsProvider = spellsProvider;
            spellNamesCache = new Dictionary<BookSpell, string>();
        }

        public IItem Generate(ItemRareness rareness)
        {
            switch (rareness)
            {
                case ItemRareness.Trash:
                case ItemRareness.Common:
                    return null;
                case ItemRareness.Uncommon:
                    return GenerateScroll(rareness, spellsProvider.GetUncommonSpells(), UncommonMinDamage, UncommonMaxDamage);
                case ItemRareness.Rare:
                    return GenerateScroll(rareness, spellsProvider.GetRareSpells(), RareMinDamage, RareMaxDamage);
                case ItemRareness.Epic:
                    throw new ArgumentException("Scrolls generator cannot generate scroll with Epic rareness.");
                default:
                    throw new ArgumentException($"Unknown rareness: {rareness}");
            }
        }

        private IItem GenerateScroll(ItemRareness rareness, BookSpell[] spells, int minDamage, int maxDamage)
        {
            var spell = RandomHelper.GetRandomElement(spells);
            var damage = RandomHelper.GetRandomValue(minDamage, maxDamage);
            var name = GetName(spell);

            return new AncientScrollItemImpl(new AncientScrollItemConfiguration
            {
                Name = $"{name} Scroll",
                Code = spell.Code,
                Mana = spell.ManaCost,
                DamagePercent = damage,
                Key = Guid.NewGuid().ToString(),
                Rareness = rareness,
                SpellName = name
            });
        }

        private string GetName(BookSpell spell)
        {
            if (spellNamesCache.ContainsKey(spell))
                return spellNamesCache[spell];

            var name = GenerateName();
            spellNamesCache.Add(spell, name);
            return name;
        }

        private static readonly char[] CapitalLetters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z', 'Ç', 'Ä', 'Å', 'È', 'Æ', 'Ö', 'Ü', 'Γ', 'Σ', 'Θ',
            'Ω', 'Φ', 'Π', 'Ñ'
        };

        private static readonly char[] SmallLetters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
            'm', 'm', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', 'ϋ', 'έ', 'â', 'ä', 'à', 'å', 'ç', 'ê', 'ë', 'è',
            'ï', 'î', 'ì', 'æ', 'ô', 'ö', 'ò', 'û', 'ù', 'ÿ', 'α', 'β',
            'π', 'σ', 'μ', 'γ', 'θ', 'δ', 'φ', 'ε', ' ', '\'', '`', 'ñ'
        };

        private const int MinNameLength = 4;
        private const int MaxNameLength = 10;

        private string GenerateName()
        {
            var nameLength = RandomHelper.GetRandomValue(MinNameLength, MaxNameLength);

            var name = RandomHelper.GetRandomElement(CapitalLetters).ToString();
            for (var counter = 1; counter < nameLength; counter++)
            {
                name += RandomHelper.GetRandomElement(SmallLetters);
            }

            return name;
        }
    }
}