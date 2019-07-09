using System.ComponentModel;
using System.Drawing;
using CodeMagic.Core.Items;

namespace CodeMagic.UI.Console.Drawing
{
    public static class ItemDrawingHelper
    {
        public static Color GetItemNameColor(IItem item)
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
                    return Color.CornflowerBlue;
                case ItemRareness.Epic:
                    return Color.BlueViolet;
                default:
                    throw new InvalidEnumArgumentException($"Unknown item rareness: {item.Rareness}");
            }
        }
    }
}