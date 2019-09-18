using System;
using System.Threading.Tasks;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Game.Locations
{
    public class SimpleLocation : ILocation
    {
        public SimpleLocation(string id, IAreaMap currentArea, Point playerPosition, bool canCast = true, bool canFight = true, bool keepOnLeave = true)
        {
            Id = id;
            CurrentArea = currentArea;
            CanCast = canCast;
            CanFight = canFight;
            KeepOnLeave = keepOnLeave;
            PlayerPosition = playerPosition;
        }

        public Point PlayerPosition { get; set; }

        public string Id { get; }

        public bool CanCast { get; }

        public bool CanFight { get; }

        public bool KeepOnLeave { get; }

        public IAreaMap CurrentArea { get; }

        public int TurnCycle => 1;

        public Task BackgroundUpdate(DateTime gameTime, int turnsCount)
        {
            var journal = new BackgroundJournal();

            return new Task(() =>
            {
                for (int counter = 0; counter < turnsCount; counter++)
                {
                    CurrentArea.PreUpdate(journal);
                    CurrentArea.Update(journal, gameTime);
                }
            });
        }
    }
}