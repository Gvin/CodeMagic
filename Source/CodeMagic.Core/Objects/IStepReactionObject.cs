using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects
{
    public interface IStepReactionObject
    {
        Point ProcessStepOn(Point position, ICreatureObject target, Point initialTargetPosition);
    }
}