namespace CodeMagic.ItemsGeneration.Configuration.Weapon.Swords
{
    public interface IBladeWeaponConfiguration
    {
        ISwordImagesConfiguration Images { get; }

        IWeightConfiguration[] Weight { get; }

        IWeaponRarenessConfiguration[] RarenessConfiguration { get; }
    }
}