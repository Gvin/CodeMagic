namespace CodeMagic.Core.Statuses
{
    public interface IBurningRelatedStatus : IObjectStatus
    {
        double CatchFireChanceModifier { get; }

        double SelfExtinguishChanceModifier { get; }
    }
}