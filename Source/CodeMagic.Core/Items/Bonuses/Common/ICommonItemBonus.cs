namespace CodeMagic.Core.Items.Bonuses.Common
{
    public interface ICommonItemBonus : IItemBonus
    {
        int WeightDecrease { get; }
    }
}