using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game
{
    public interface IGameCore : IDisposable
    {
        IAreaMap Map { get; }

        void ChangeMap(IAreaMap newMap, Point playerPosition);

        Journal Journal { get; }

        Point PlayerPosition { get; }

        IPlayer Player { get; }

        AreaMapFragment GetVisibleArea();

        void PerformPlayerAction(IPlayerAction action);
    }
}