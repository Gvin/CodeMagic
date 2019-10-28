using CodeMagic.Core.Objects;

namespace CodeMagic.Core.CreaturesLogic.TargetStrategies
{
    public class PlayerTargetStrategy : ITargetStrategy
    {
        public bool IsTarget(IMapObject mapObject)
        {
            return mapObject is IPlayer;
        }
    }
}