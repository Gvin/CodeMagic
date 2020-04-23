using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon
{
    public interface IWeaponsConfiguration
    {
        IWeaponConfiguration SwordsConfiguration { get; }

        IWeaponConfiguration DaggersConfiguration { get; }

        IWeaponConfiguration MacesConfiguration { get; }

        IWeaponConfiguration AxesConfiguration { get; }

        IWeaponConfiguration StaffsConfiguration { get; }

        IDescriptionConfiguration DescriptionConfiguration { get; }
    }
}