using CodeMagic.Core.Area;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.CreaturesLogic.Strategies
{
    public class StandStillStrategy : ICreatureStrategy
    {
        public bool Update(INonPlayableCreatureObject creature, IAreaMap map, IJournal journal, Point position)
        {
            return true;
        }
    }
}