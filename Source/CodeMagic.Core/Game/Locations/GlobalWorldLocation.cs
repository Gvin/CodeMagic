using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Game.Locations
{
    public class GlobalWorldLocation : ILocation
    {
        private const int WorldMoveTurnCycle = 20;

        private int turnsCounter;
        private Point enterObjectPosition;

        public GlobalWorldLocation(string id, IAreaMap worldMap, Point playerHomePosition)
        {
            CurrentArea = worldMap;
            turnsCounter = 0;
            Id = id;
            enterObjectPosition = playerHomePosition;
        }

        public string Id { get; }

        public string Name => "World";

        public bool CanBuild => false;

        public IAreaMap CurrentArea { get; }

        private void StorePlayerPosition(Point position, Direction direction)
        {
            enterObjectPosition = Point.GetPointInDirection(position, direction);
        }

        public Point GetEnterPoint(Direction direction)
        {
            return Point.GetPointInDirection(enterObjectPosition, DirectionHelper.InvertDirection(direction));
        }

        public void BackgroundUpdate(DateTime gameTime)
        {
            turnsCounter++;
            var journal = new BackgroundJournal();

            while (turnsCounter >= TurnCycle)
            {
                turnsCounter -= TurnCycle;
                CurrentArea.PreUpdate(journal);
                CurrentArea.Update(journal, gameTime);
            }
        }

        public bool KeepOnLeave => true;

        public bool CanCast => false;

        public bool CanFight => false;

        public int TurnCycle => WorldMoveTurnCycle;
        public void ProcessPlayerEnter(IGameCore game)
        {
            // Do nothing
        }

        public void ProcessPlayerLeave(IGameCore game)
        {
            StorePlayerPosition(game.PlayerPosition, game.Player.Direction);
        }
    }
}