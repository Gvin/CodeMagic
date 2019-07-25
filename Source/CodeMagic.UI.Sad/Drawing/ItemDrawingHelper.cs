using System;
using CodeMagic.Core.Items;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Sad.Drawing
{
    public static class ItemDrawingHelper
    {
        public static Color GetItemColor(IItem item)
        {
            switch (item.Rareness)
            {
                case ItemRareness.Trash:
                    return Color.Gray;
                case ItemRareness.Common:
                    return Color.White;
                case ItemRareness.Uncommon:
                    return Color.Lime;
                case ItemRareness.Rare:
                    return Color.LightBlue;
                case ItemRareness.Epic:
                    return Color.Violet;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}