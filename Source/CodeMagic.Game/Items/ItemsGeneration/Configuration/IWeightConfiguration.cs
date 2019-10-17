namespace CodeMagic.Game.Items.ItemsGeneration.Configuration
{
    public interface IWeightConfiguration
    {
        ItemMaterial Material { get; }

        int Weight { get; }
    }
}