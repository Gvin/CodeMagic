namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Tool
{
    public interface IToolConfiguration
    {
        string InventoryImageTemplate { get; }

        IWeightConfiguration[] Weight { get; }

        IToolRarenessConfiguration[] RarenessConfiguration { get; }
    }
}