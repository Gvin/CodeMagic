using CodeMagic.ItemsGeneration.Configuration.Armor;
using CodeMagic.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.ItemsGeneration.Configuration
{
    public interface IItemGeneratorConfiguration
    {
        IWeaponConfiguration WeaponConfiguration { get; }

        IArmorConfiguration ArmorConfiguration { get; }
    }
}