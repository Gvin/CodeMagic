using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Objects.Creatures
{
    public interface INonPlayableCreatureObject : ICreatureObject, IDynamicObject
    {
        void Attack(IDestroyableObject target, IJournal journal);
    }
}