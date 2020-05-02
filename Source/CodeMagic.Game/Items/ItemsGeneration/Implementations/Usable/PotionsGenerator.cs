using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Usable;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Usable
{
    public class PotionsGenerator : IUsableItemTypeGenerator
    {
        private readonly Dictionary<PotionColor, PotionType> potionTypes;

        public PotionsGenerator()
        {
            potionTypes = new Dictionary<PotionColor, PotionType>();
            InitializePotionTypes();
        }

        private void InitializePotionTypes()
        {
            potionTypes.Clear();

            var colors = Enum.GetValues(typeof(PotionColor)).Cast<PotionColor>().ToList();
            var types = Enum.GetValues(typeof(PotionType)).Cast<PotionType>().ToList();

            foreach (var potionColor in colors)
            {
                var type = RandomHelper.GetRandomElement(types.ToArray());
                types.Remove(type);
                potionTypes.Add(potionColor, type);
            }
        }

        public void Reset()
        {
            InitializePotionTypes();
        }

        public IItem Generate(ItemRareness rareness)
        {
            var color = RandomHelper.GetRandomEnumValue<PotionColor>();
            var type = potionTypes[color];
            var size = GetSize(rareness);
            
            return new Potion(color, type, size, rareness);
        }

        private static PotionSize GetSize(ItemRareness rareness)
        {
            switch (rareness)
            {
                case ItemRareness.Common:
                    return PotionSize.Small;
                case ItemRareness.Uncommon:
                    return PotionSize.Medium;
                case ItemRareness.Rare:
                    return PotionSize.Big;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rareness), rareness, null);
            }
        }
    }
}