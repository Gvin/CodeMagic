namespace CodeMagic.Core.Statuses
{
    public interface IBurningRelatedStatus
    {
        int CatchFireChanceModifier { get; }

        int SelfExtinguishChanceModifier { get; }
    }
}