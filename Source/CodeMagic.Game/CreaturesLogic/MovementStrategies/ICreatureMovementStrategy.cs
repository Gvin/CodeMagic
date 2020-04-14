using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.CreaturesLogic.MovementStrategies
{
    public interface ICreatureMovementStrategy
    {
        bool TryMove(ICreatureObject creature, Point position, Point targetPosition);
    }
}