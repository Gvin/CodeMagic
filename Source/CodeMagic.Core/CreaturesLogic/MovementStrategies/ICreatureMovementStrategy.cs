using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic.MovementStrategies
{
    public interface ICreatureMovementStrategy
    {
        bool TryMove(ICreatureObject creature, IAreaMap map, Point position, Point targetPosition);
    }
}