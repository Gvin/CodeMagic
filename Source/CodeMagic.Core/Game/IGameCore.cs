using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game
{
    public interface IGameCore
    {
        IAreaMap Map { get; }

        Journal Journal { get; }

        Point PlayerPosition { get; }

        IPlayer Player { get; }
    }
}