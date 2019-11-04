using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon.Blade;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon.Head;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon
{
    public interface IWeaponConfiguration
    {
        IBladeWeaponConfiguration SwordsConfiguration { get; }

        IBladeWeaponConfiguration DaggersConfiguration { get; }

        IHeadWeaponConfiguration MacesConfiguration { get; }

        IHeadWeaponConfiguration AxesConfiguration { get; }

        IHeadWeaponConfiguration StaffsConfiguration { get; }

        IDescriptionConfiguration DescriptionConfiguration { get; }
    }
}