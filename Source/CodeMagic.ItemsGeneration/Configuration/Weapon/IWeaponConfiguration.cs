using CodeMagic.ItemsGeneration.Configuration.Weapon.Swords;

namespace CodeMagic.ItemsGeneration.Configuration.Weapon
{
    public interface IWeaponConfiguration
    {
        IBladeWeaponConfiguration SwordsConfiguration { get; }

        IBladeWeaponConfiguration DaggersConfiguration { get; }
    }
}