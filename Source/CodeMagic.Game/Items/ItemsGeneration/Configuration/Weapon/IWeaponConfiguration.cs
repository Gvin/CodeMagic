namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon
{
    public interface IWeaponConfiguration
    {
        IWeaponImagesConfiguration Images { get; }

        string EquippedImageRight { get; }

        string EquippedImageLeft { get; }

        IWeightConfiguration[] Weight { get; }

        IWeaponRarenessConfiguration[] RarenessConfiguration { get; }
    }
}