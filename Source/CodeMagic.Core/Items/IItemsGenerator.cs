using CodeMagic.Core.Injection;

namespace CodeMagic.Core.Items
{
    public interface IItemsGenerator : IInjectable
    {
        WeaponItem GenerateWeapon(ItemRareness rareness);

        ArmorItem GenerateArmor(ItemRareness rareness);

        SpellBook GenerateSpellBook(ItemRareness rareness);
    }
}