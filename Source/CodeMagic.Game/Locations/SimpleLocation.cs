using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Game.Locations
{
    public class SimpleLocation : IGameLocation
    {
        private readonly Dictionary<Direction, Point> enterPositions;

        public SimpleLocation(
            string id, 
            string name,
            IAreaMap currentArea, 
            Dictionary<Direction, Point> enterPositions, 
            bool canBuild = false,
            bool canCast = true, 
            bool canFight = true, 
            bool canUse = true,
            bool keepOnLeave = true)
        {
            Id = id;
            Name = name;
            CurrentArea = currentArea;
            CanCast = canCast;
            CanFight = canFight;
            CanBuild = canBuild;
            CanUse = canUse;
            KeepOnLeave = keepOnLeave;
            this.enterPositions = enterPositions.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public string Id { get; }

        public bool CanBuild { get; }

        public string Name { get; }

        public bool CanCast { get; }

        public bool CanFight { get; }
        public bool CanUse { get; }

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