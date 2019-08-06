namespace CodeMagic.ItemsGeneration.Configuration.Weapon.Description
{
    public interface IWeaponDescriptionConfiguration
    {
        IRarenessDescriptionConfiguration[] RarenessDescription { get; }

        IMaterialDescriptionConfiguration[] MaterialDescription { get; }
    }
}