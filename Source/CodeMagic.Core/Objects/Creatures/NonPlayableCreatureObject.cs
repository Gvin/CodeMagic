using CodeMagic.Core.Area;
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
        }

        public Direction Direction { get; set; }

        public void Update(IAreaMap map, Point position, Journal journal)
        {
            Logic.Update(this, map, position, journal);
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