using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Game.Locations
{
    public class SimpleLocation : ILocation
    {
        private readonly Dictionary<Direction, Point> enterPositions;

        public SimpleLocation(
            string id, 
            IAreaMap currentArea, 
            Dictionary<Direction, Point> enterPositions, 
            bool canCast = true, 
            bool canFight = true, 
            bool keepOnLeave = true)
        {
            Id = id;
            CurrentArea = currentArea;
            CanCast = canCast;
            CanFight = canFight;
            KeepOnLeave = keepOnLeave;
            this.enterPositions = enterPositions.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public string Id { get; }

        public bool CanCast { get; }

        public bool CanFight { get; }

        public bool KeepOnLeave { get; }

        public IAreaMap CurrentArea { get; }

        public int TurnCycle => 1;
        public void ProcessPlayerEnter(IGameCore game)
        {
            // Do nothing
        }

        public void ProcessPlayerLeave(IGameCore game)
        {
            // Do nothing
        }

        public Point GetEnterPoint(Direction direction)
        {
            return enterPositions[direction];
        }

        public void BackgroundUpdate(DateTime gameTime)
        {
            var journal = new BackgroundJournal();

            CurrentArea.PreUpdate(journal);
            CurrentArea.Update(journal, gameTime);
        }
    }
}