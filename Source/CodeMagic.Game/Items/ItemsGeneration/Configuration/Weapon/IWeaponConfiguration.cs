namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon
{
    public interface IWeaponConfiguration
    {
        ILayersImagesConfiguration Images { get; }

        string EquippedImageRight { get; }

        string EquippedImageLeft { get; }

        IWeightConfiguration[] Weight { get; }

        IWeaponRarenessConfiguration[] RarenessConfiguration { get; }
    }
}