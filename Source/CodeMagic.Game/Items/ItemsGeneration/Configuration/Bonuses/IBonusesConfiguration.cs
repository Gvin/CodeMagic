namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses
{
    public interface IBonusesConfiguration
    {
        IItemGroupBonusesConfiguration[] Groups { get; }
    }
}