using CodeMagic.Core.Common;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Objects.Creatures
{
    public abstract class NonPlayableCreatureObject : DestroyableObject, INonPlayableCreatureObject
    {
        public NonPlayableCreatureObject(NonPlayableCreatureObjectConfiguration configuration) 
            : base(configuration)
        {
            Logic = new Logic();
            Direction = Direction.Up;
        }

        public Direction Direction { get; set; }

        public void Update(IGameCore game, Point position)
        {
            Logic.Update(this, game, position);
        }

        public virtual void Attack(IDestroyableObject target, Journal journal)
        {
        }

        public bool Updated { get; set; }

        protected Logic Logic { get; }
    }

    public class NonPlayableCreatureObjectConfiguration : DestroyableObjectConfiguration
    {
    }
}