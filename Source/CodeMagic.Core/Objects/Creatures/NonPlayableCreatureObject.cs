using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Objects.Creatures
{
    public abstract class NonPlayableCreatureObject : CreatureObject, INonPlayableCreatureObject
    {
        public NonPlayableCreatureObject(NonPlayableCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            Logic = new Logic();
        }

        public void Update(IGameCore game, Point position)
        {
            Logic.Update(this, game, position);
        }

        public virtual void Attack(IDestroyableObject target, Journal journal)
        {
        }

        public bool Updated { get; set; }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        protected Logic Logic { get; }
    }

    public class NonPlayableCreatureObjectConfiguration : CreatureObjectConfiguration
    {
    }
}