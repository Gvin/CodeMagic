using System;

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
    }
}