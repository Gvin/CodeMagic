using System;
using System.Threading.Tasks;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;

namespace CodeMagic.Core.Game.Locations
{
    public interface ILocation
    {
        string Name { get; }

        string Id { get; }

        IAreaMap CurrentArea { get; }

        Point GetEnterPoint(Direction direction);

        void BackgroundUpdate(DateTime gameTime);

        bool KeepOnLeave { get; }

        bool CanCast { get; }

        bool CanFight { get; }

        int TurnCycle { get; }

        void ProcessPlayerEnter(IGameCore game);

        void ProcessPlayerLeave(IGameCore game);
    }
}