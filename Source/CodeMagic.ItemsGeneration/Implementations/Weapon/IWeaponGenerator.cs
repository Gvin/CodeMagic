using CodeMagic.Core.Items;

namespace CodeMagic.ItemsGeneration.Implementations.Weapon
{
    internal interface IWeaponGenerator
    {
        WeaponItem GenerateWeapon(ItemRareness rareness);
    }
}