using CodeMagic.Core.Common;

namespace CodeMagic.Core.Objects.Creatures
{
    public interface ICreatureObject : IDestroyableObject
    {
        Direction Direction { get; set; }
    }
}