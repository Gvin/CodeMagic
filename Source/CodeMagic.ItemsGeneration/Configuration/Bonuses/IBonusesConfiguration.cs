namespace CodeMagic.ItemsGeneration.Configuration.Bonuses
{
    public interface IBonusesConfiguration
    {
        IItemGroupBonusesConfiguration[] Groups { get; }
    }
}