using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items
{
    public interface IItemsGenerator : IInjectable
    {
        WeaponItem GenerateWeapon(ItemRareness rareness);

        ArmorItem GenerateArmor(ItemRareness rareness, ArmorClass armorClass);

        SpellBook GenerateSpellBook(ItemRareness rareness);

        IItem GenerateUsable(ItemRareness rareness);

        IItem GenerateResource();

        IItem GenerateLumberjackAxe(ItemRareness rareness);

        IItem GeneratePickaxe(ItemRareness rareness);
    }
}