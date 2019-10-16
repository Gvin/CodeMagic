namespace CodeMagic.Core.Statuses
{
    public interface IBurningRelatedStatus
    {
        double CatchFireChanceModifier { get; }

        double SelfExtinguishChanceModifier { get; }
    }
}