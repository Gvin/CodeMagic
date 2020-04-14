using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.CreaturesLogic.Strategies
{
    public class StandStillStrategy : ICreatureStrategy
    {
        public bool Update(INonPlayableCreatureObject creature, Point position)
        {
            return true;
        }
    }
}