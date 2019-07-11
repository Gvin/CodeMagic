using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects
{
    public interface IStepReactionObject
    {
        Point ProcessStepOn(IGameCore game, Point position, ICreatureObject target, Point initialTargetPosition);
    }
}