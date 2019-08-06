namespace CodeMagic.ItemsGeneration.Configuration
{
    public interface IWeightConfiguration
    {
        ItemMaterial Material { get; }

        int Weight { get; }
    }
}