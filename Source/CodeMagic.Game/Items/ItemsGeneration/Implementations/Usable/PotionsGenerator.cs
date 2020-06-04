using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Usable;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Usable
{
    public class PotionsGenerator : IUsableItemTypeGenerator
    {
        public IItem Generate(ItemRareness rareness)
        {
            var color = RandomHelper.GetRandomEnumValue<PotionColor>();
            var type = GameData.Current.PotionsPattern[color];
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