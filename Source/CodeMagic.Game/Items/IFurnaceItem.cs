using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items
{
    public interface IFurnaceItem : IItem
    {
        int MinTemperature { get; }

        int MaxTemperature { get; }

        int FurnaceProcessingTime { get; }

        IItem CreateFurnaceResult();
    }
}