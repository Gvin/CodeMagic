using System;
using System.Threading.Tasks;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Game.Locations
{
    public class GlobalWorldLocation : ILocation
    {
        private const int WorldMoveTurnCycle = 10;

        private int turnsCounter;

        public GlobalWorldLocation(string id, IAreaMap worldMap)
        {
            CurrentArea = worldMap;
            turnsCounter = 0;
            Id = id;
        }

        public string Id { get; }

        public IAreaMap CurrentArea { get; }

        public Point PlayerPosition { get; set; }

        public Task BackgroundUpdate(DateTime gameTime, int turnsCount)
        {
            turnsCounter += turnsCount;
            var journal = new BackgroundJournal();

            return new Task(() =>
            {
                while (turnsCount >= TurnCycle)
                {
                    turnsCounter -= TurnCycle;
                    CurrentArea.PreUpdate(journal);
                    CurrentArea.Update(journal, gameTime);
                }
            });
        }

        public bool KeepOnLeave => true;

        public bool CanCast => false;

        public bool CanFight => false;

        public int TurnCycle => WorldMoveTurnCycle;
    }
}