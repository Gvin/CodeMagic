using System;
using System.Drawing;
using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items
{
    public static class ItemDrawingHelper
    {
        public static Color GetItemColor(IItem item)
        {
            return GetRarenessColor(item.Rareness);
        }

        public static Color GetRarenessColor(ItemRareness rareness)
        {
            switch (rareness)
            {
                case ItemRareness.Trash:
                    return Color.Gray;
                case ItemRareness.Common:
                    return Color.White;
                case ItemRareness.Uncommon:
                    return Color.Lime;
                case ItemRareness.Rare:
                    return Color.FromArgb(0, 102, 255);
                case ItemRareness.Epic:
                    return Color.Violet;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}