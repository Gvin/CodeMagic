namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Description
{
    public interface IMaterialDescriptionConfiguration
    {
        ItemMaterial Material { get; }

        string[] Text { get; }
    }
}