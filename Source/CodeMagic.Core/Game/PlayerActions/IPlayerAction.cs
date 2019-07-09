using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game.PlayerActions
{
    public interface IPlayerAction
    {
        bool Perform(IPlayer player, Point playerPosition, IAreaMap map, Journal journal);
    }
}