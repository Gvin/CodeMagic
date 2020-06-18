namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield
{
    public interface IShieldConfiguration
    {
        ILayersImagesConfiguration Images { get; }

        string Name { get; }

        string WorldImage { get; }

        string EquippedImageRight { get; }

        string EquippedImageLeft { get; }

        IWeightConfiguration[] Weight { get; }

        IShieldRarenessConfiguration[] RarenessConfiguration { get; }
    }
}