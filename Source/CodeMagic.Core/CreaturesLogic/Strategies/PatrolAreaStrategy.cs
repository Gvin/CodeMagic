﻿using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic.Strategies
{
    public class PatrolAreaStrategy : ICreatureStrategy
    {
        private Direction? currentDirection;

        public bool Update(INonPlayableCreatureObject creature, IAreaMap map, IJournal journal, Point position)
        {
            if (!currentDirection.HasValue)
            {
                currentDirection = GenerateDirection();
            }

            var targetPoint = Point.GetPointInDirection(position, currentDirection.Value);
            var movementResult = MovementHelper.MoveCreature(creature, map, journal, position, targetPoint, true, true);
            if (!movementResult.Success)
            {
                currentDirection = null;
            }

            return true;
        }

        private Direction GenerateDirection()
        {
            return RandomHelper.GetRandomElement(Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray());
        }
    }
}