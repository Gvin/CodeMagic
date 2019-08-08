using CodeMagic.ItemsGeneration.Configuration.Description;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Blade;
using CodeMagic.ItemsGeneration.Configuration.Weapon.Head;

namespace CodeMagic.ItemsGeneration.Configuration.Weapon
{
    public interface IWeaponConfiguration
    {
        IBladeWeaponConfiguration SwordsConfiguration { get; }

        IBladeWeaponConfiguration DaggersConfiguration { get; }

        IHeadWeaponConfiguration MacesConfiguration { get; }

        IHeadWeaponConfiguration AxesConfiguration { get; }

        IDescriptionConfiguration DescriptionConfiguration { get; }
    }
}