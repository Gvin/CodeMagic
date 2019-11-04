using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Weapon
{
    internal interface IWeaponGenerator
    {
        WeaponItem GenerateWeapon(ItemRareness rareness);
    }
}