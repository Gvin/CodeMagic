namespace CodeMagic.Game.Items.ItemsGeneration.Configuration
{
    public interface ILayersImagesConfiguration
    {
        ILayersImageSpriteConfiguration[] Sprites { get; }
    }

    public interface ILayersImageSpriteConfiguration
    {
        int Index { get; }

        string[] Images { get; }
    }
}