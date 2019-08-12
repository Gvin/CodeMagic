using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.Core.Objects.Creatures
{
    public interface INonPlayableCreatureObject : ICreatureObject, IDynamicObject
    {
        void Attack(IDestroyableObject target, Journal journal);

        RemainsType RemainsType { get; }
    }
}