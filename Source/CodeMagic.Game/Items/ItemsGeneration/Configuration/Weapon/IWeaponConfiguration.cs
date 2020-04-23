namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon
{
    public interface IWeaponConfiguration
    {
        IWeaponImagesConfiguration Images { get; }

        IWeightConfiguration[] Weight { get; }

        IWeaponRarenessConfiguration[] RarenessConfiguration { get; }
    }
}