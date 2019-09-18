using System;
using System.Threading.Tasks;
using CodeMagic.Core.Area;

namespace CodeMagic.Core.Game.Locations
{
    public interface ILocation
    {
        string Id { get; }

        IAreaMap CurrentArea { get; }

        Point PlayerPosition { get; set; }

        Task BackgroundUpdate(DateTime gameTime, int turnsCount);

        bool KeepOnLeave { get; }

        bool CanCast { get; }

        bool CanFight { get; }

        int TurnCycle { get; }
    }
}