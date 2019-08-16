using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game
{
    public interface IGameCore
    {
        int Level { get; }

        IAreaMap Map { get; }

        Journal Journal { get; }

        Point PlayerPosition { get; }

        IPlayer Player { get; }

        AreaMapFragment GetVisibleArea();

        void PerformPlayerAction(IPlayerAction action);

        void GoToNextLevel();
    }
}