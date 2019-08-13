using CodeMagic.Core.Injection;

namespace CodeMagic.Core.Items
{
    public interface IItemsGenerator : IInjectable
    {
        WeaponItem GenerateWeapon(ItemRareness rareness);

        ArmorItem GenerateArmor(ItemRareness rareness, ArmorClass[] classesWhitelist);

        SpellBook GenerateSpellBook(ItemRareness rareness);

        Item GeneratePotion(ItemRareness rareness);
    }
}