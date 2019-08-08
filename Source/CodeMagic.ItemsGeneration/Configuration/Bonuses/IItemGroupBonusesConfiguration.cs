namespace CodeMagic.ItemsGeneration.Configuration.Bonuses
{
    public interface IItemGroupBonusesConfiguration
    {
        string Type { get; }

        string InheritFrom { get; }

        IBonusRarenessConfiguration[] Configuration { get; }
    }
}