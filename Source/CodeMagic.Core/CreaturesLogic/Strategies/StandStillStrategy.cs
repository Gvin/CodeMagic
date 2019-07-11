using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic.Strategies
{
    public class StandStillStrategy : ICreatureStrategy
    {
        public bool Update(INonPlayableCreatureObject creature, IGameCore game, Point position)
        {
            return true;
        }
    }
}