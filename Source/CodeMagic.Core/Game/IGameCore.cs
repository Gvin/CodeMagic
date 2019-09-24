using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Locations;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game
{
    public interface IGameCore : IDisposable
    {
        DateTime GameTime { get; }

        GameWorld World { get; }

        IAreaMap Map { get; }

        Journal Journal { get; }

        Point PlayerPosition { get; }

        IPlayer Player { get; }

        AreaMapFragment GetVisibleArea();

        void PerformPlayerAction(IPlayerAction action);

        void RemovePlayerFromMap();

        void UpdatePlayerPosition(Point newPlayerPosition);
    }
}