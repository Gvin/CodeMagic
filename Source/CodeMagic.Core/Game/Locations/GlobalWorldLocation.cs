﻿using System;
using System.Threading.Tasks;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Game.Locations
{
    public class GlobalWorldLocation : ILocation
    {
        private const int WorldMoveTurnCycle = 10;

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

        public IAreaMap CurrentArea { get; }

        private void StorePlayerPosition(Point position, Direction direction)
        {
            enterObjectPosition = Point.GetPointInDirection(position, direction);
        }

        public Point GetEnterPoint(Direction direction)
        {
            return Point.GetPointInDirection(enterObjectPosition, DirectionHelper.InvertDirection(direction));
        }

        public Task BackgroundUpdate(DateTime gameTime, int turnsCount)
        {
            turnsCounter += turnsCount;
            var journal = new BackgroundJournal();

            return new Task(() =>
            {
                while (turnsCounter >= TurnCycle)
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