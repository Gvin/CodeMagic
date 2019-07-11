using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic.Strategies
{
    public interface ICreatureStrategy
    {
        bool Update(INonPlayableCreatureObject creature, IGameCore game, Point position);
    }
}