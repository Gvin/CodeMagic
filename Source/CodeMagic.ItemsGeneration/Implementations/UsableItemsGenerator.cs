using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations;
using CodeMagic.ItemsGeneration.Implementations.Usable;

namespace CodeMagic.ItemsGeneration.Implementations
{
    public class UsableItemsGenerator
    {
        private readonly Dictionary<UsableItemType, IUsableItemTypeGenerator> generators;

        public UsableItemsGenerator(IImagesStorage imagesStorage, IAncientSpellsProvider spellsProvider)
        {
            generators = new Dictionary<UsableItemType, IUsableItemTypeGenerator>
            {
                {UsableItemType.HealthPotion, new HealthPotionsGenerator(imagesStorage)},
                {UsableItemType.ManaPotion, new ManaPotionsGenerator(imagesStorage)},
                {UsableItemType.RestorationPotion, new RestorationPotionsGenerator(imagesStorage)},
                {UsableItemType.Scroll, new ScrollsGenerator(spellsProvider)}
            };
        }

        public IItem GenerateUsableItem(ItemRareness rareness)
        {
            var type = GetRandomItemType();
            if (generators.ContainsKey(type))
            {
                return generators[type].Generate(rareness);
            }

            throw new ArgumentException($"Unknown usable item type: {type}");
        }

        private UsableItemType GetRandomItemType()
        {
            var types = Enum.GetValues(typeof(UsableItemType)).OfType<UsableItemType>().ToArray();
            return RandomHelper.GetRandomElement(types);
        }

        private enum UsableItemType
        {
            HealthPotion,
            ManaPotion,
            RestorationPotion,
            Scroll
        }
    }
}