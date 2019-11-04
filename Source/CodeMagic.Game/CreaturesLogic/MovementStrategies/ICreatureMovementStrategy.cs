using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.CreaturesLogic.MovementStrategies
{
    public interface ICreatureMovementStrategy
    {
        bool TryMove(ICreatureObject creature, IAreaMap map, IJournal journal, Point position, Point targetPosition);
    }
}