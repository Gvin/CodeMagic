namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon
{
    public interface IWeaponImagesConfiguration
    {
        IWeaponImageSpriteConfiguration[] Sprites { get; }
    }

    public interface IWeaponImageSpriteConfiguration
    {
        int Index { get; }

        string[] Images { get; }
    }
}