using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic
{
    public interface ICreatureStrategy
    {
        bool Update(INonPlayableCreatureObject creature, Point position);
    }
}