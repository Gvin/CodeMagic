using CodeMagic.Core.Objects;

namespace CodeMagic.Core.CreaturesLogic.TargetStrategies
{
    public interface ITargetStrategy
    {
        bool IsTarget(IMapObject mapObject);
    }
}