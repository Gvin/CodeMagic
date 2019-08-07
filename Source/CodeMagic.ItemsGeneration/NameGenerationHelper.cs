using System;
using CodeMagic.Core.Items;

namespace CodeMagic.ItemsGeneration
{
    public static class NameGenerationHelper
    {
        public static string GetMaterialPrefix(ItemMaterial material)
        {
            switch (material)
            {
                case ItemMaterial.Wood:
                    return "Wooden";
                case ItemMaterial.Leather:
                    return "Leather";
                case ItemMaterial.Iron:
                    return "Iron";
                case ItemMaterial.Steel:
                    return "Steel";
                case ItemMaterial.Silver:
                    return "Silver";
                case ItemMaterial.ElvesMetal:
                    return "Elven";
                case ItemMaterial.DwarfsMetal:
                    return "Dwarf";
                case ItemMaterial.Mythril:
                    return "Mythril";
                default:
                    throw new ArgumentException($"Unknown material: {material}");
            }
        }

        public static string GetRarenessPrefix(ItemRareness rareness)
        {
            switch (rareness)
            {
                case ItemRareness.Trash:
                    return "Weak";
                case ItemRareness.Common:
                    return string.Empty;
                case ItemRareness.Uncommon:
                    return "Good";
                case ItemRareness.Rare:
                    return "Excellent";
                default:
                    throw new ArgumentException($"Unsupported rareness: {rareness}");
            }
        }
    }
}