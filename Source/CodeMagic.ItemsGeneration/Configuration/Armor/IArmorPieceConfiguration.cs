namespace CodeMagic.ItemsGeneration.Configuration.Armor
{
    public interface IArmorPieceConfiguration
    {
        string TypeName { get; }

        string[] Images { get; }

        IArmorRarenessConfiguration[] RarenessConfigurations { get; }

        IWeightConfiguration[] Weight { get; }
    }
}