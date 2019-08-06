using CodeMagic.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.ItemsGeneration.Configuration
{
    public interface IItemGeneratorConfiguration
    {
        IWeaponConfiguration WeaponConfiguration { get; }
    }
}