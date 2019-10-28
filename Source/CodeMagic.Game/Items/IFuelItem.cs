using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.Items
{
    public interface IFuelItem : IItem, IFuelObject
    {
        int MaxFuel { get; }
    }
}