using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Items
{
    public interface IFuelItem : IItem, IFuelObject
    {
        int MaxFuel { get; }
    }
}