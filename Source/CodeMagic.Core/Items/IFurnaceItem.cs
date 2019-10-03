namespace CodeMagic.Core.Items
{
    public interface IFurnaceItem : IItem
    {
        int MinTemperature { get; }

        int MaxTemperature { get; }

        int FurnaceProcessingTime { get; }

        IItem CreateFurnaceResult();
    }
}