using CodeMagic.Core.Items;

namespace CodeMagic.ItemsGeneration.Implementations
{
    internal interface IWeaponGenerator
    {
        WeaponItem GenerateWeapon(ItemRareness rareness);
    }
}