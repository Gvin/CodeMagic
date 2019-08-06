namespace CodeMagic.ItemsGeneration.Configuration.Weapon.Head
{
    public interface IHeadWeaponConfiguration
    {
        IHeadImagesConfiguration Images { get; }

        IWeightConfiguration[] Weight { get; }

        IWeaponRarenessConfiguration[] RarenessConfiguration { get; }
    }
}