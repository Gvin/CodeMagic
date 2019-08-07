namespace CodeMagic.Core.Items.Bonuses.Common
{
    public class WeightDecreaseItemBonus : ICommonItemBonus
    {
        public WeightDecreaseItemBonus(int weightDecrease)
        {
            WeightDecrease = weightDecrease;
        }

        public int WeightDecrease { get; }
    }
}