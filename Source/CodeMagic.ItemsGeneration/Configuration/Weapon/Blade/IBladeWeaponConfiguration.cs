namespace CodeMagic.ItemsGeneration.Configuration.Weapon.Blade
{
    public interface IBladeWeaponConfiguration
    {
        IBladeImagesConfiguration Images { get; }

        IWeightConfiguration[] Weight { get; }

        IWeaponRarenessConfiguration[] RarenessConfiguration { get; }
    }
}