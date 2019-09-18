using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;

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