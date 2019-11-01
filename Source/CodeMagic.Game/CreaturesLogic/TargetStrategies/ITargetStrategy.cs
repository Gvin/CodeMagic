using CodeMagic.Core.Objects;

namespace CodeMagic.Game.CreaturesLogic.TargetStrategies
{
    public interface ITargetStrategy
    {
        bool IsTarget(IMapObject mapObject);
    }
}