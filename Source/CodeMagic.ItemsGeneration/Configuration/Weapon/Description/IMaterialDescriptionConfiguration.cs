namespace CodeMagic.ItemsGeneration.Configuration.Weapon.Description
{
    public interface IMaterialDescriptionConfiguration
    {
        ItemMaterial Material { get; }

        string[] Text { get; }
    }
}