using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game.PlayerActions
{
    public class EmptyPlayerAction : IPlayerAction
    {
        public bool Perform(IPlayer player, Point playerPosition, IAreaMap map, Journal journal)
        {
            // Do nothing.
            return true;
        }
    }
}