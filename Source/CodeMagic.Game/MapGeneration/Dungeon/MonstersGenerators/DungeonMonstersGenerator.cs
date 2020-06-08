using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.Objects.Creatures.NonPlayable;

namespace CodeMagic.Game.MapGeneration.Dungeon.MonstersGenerators
{
    internal class DungeonMonstersGenerator : IMonstersGenerator
    {
        private static readonly ILog Log = LogManager.GetLog<DungeonMonstersGenerator>();

        private const int SquadForceMultiplier = 2;
        private const double SquadForceVariation = 0.2d;
        private const double SquadsCountMultiplier = 0.01d;

        public void GenerateMonsters(IAreaMap map, Point playerPosition)
        {
            var stopwatch = Stopwatch.StartNew();

            var squadsCount = GetSquadsCount(map);

            var possibleMonsters = ConfigurationManager.Current.Monsters.Monsters
                .Where(monster => map.Level >= monster.SpawnConfiguration.MinLevel).ToArray();
            var possibleGroups = possibleMonsters.Select(monster => monster.SpawnConfiguration.Group).Distinct()
                .ToArray();

            
            var occupiedCells = new bool[map.Width,map.Height];
            var playerRoomPoints = CollectRoomPoints(map, playerPosition);
            foreach (var point in playerRoomPoints)
            {
                occupiedCells[point.X, point.Y] = true;
            }

            for (var counter = 0; counter < squadsCount; counter++)
            {
                var group = RandomHelper.GetRandomElement(possibleGroups);
                PlaceSquad(map, possibleMonsters, group, occupiedCells);
            }

            stopwatch.Stop();
            Log.Debug($"GenerateMonsters took {stopwatch.ElapsedMilliseconds} milliseconds.");
        }

        private static void PlaceSquad(IAreaMap map, IMonsterConfiguration[] monsters, string group,
            bool[,] occupiedCells)
        {
            var squadMonsters = monsters.Where(monster => string.Equals(monster.SpawnConfiguration.Group, group))
                .ToArray();

            var roomPoint = FindEmptyRoom(map, occupiedCells);
            var roomPoints = CollectRoomPoints(map, roomPoint);

            foreach (var point in roomPoints)
            {
                occupiedCells[point.X, point.Y] = true;
            }

            var emptyRoomPoints = roomPoints.ToList();
            var leftForce = GetSquadForce(map.Level);
            var monsterConfig = GenerateMonster(squadMonsters, leftForce);

            while (monsterConfig != null && emptyRoomPoints.Count > 0)
            {
                leftForce -= monsterConfig.SpawnConfiguration.Force;

                var position = RandomHelper.GetRandomElement(emptyRoomPoints.ToArray());
                emptyRoomPoints.Remove(position);
                var monster = CreateMonster(monsterConfig);
                map.AddObject(position, monster);

                monsterConfig = GenerateMonster(squadMonsters, leftForce);
            }
        }

        private static ICreatureObject CreateMonster(IMonsterConfiguration config)
        {
            return new MonsterCreatureImpl(new MonsterCreatureImplConfiguration(config));
        }

        private static int GetSquadForce(int level)
        {
            var baseValue = level * SquadForceMultiplier;
            var minValue = (int) Math.Floor(baseValue * (1d - SquadForceVariation));
            var maxValue = (int) Math.Ceiling(baseValue * (1d + SquadForceVariation));
            return RandomHelper.GetRandomValue(minValue, maxValue);
        }

        private static IMonsterConfiguration GenerateMonster(IMonsterConfiguration[] config, int leftForce)
        {
            var possibleMonsters = config.Where(monster => leftForce >= monster.SpawnConfiguration.Force).ToArray();
            if (possibleMonsters.Length == 0)
                return null;

            return RandomHelper.GetRandomElement(possibleMonsters);
        }

        private static Point[] CollectRoomPoints(IAreaMap map, Point startPoint)
        {
            var markedPoints = new bool[map.Width, map.Height];

            var leftPoints = new Stack<Point>();
            leftPoints.Push(startPoint);
            while (leftPoints.Count > 0)
            {
                var point = leftPoints.Pop();
                if (markedPoints[point.X, point.Y])
                    continue;

                var cell = map.TryGetCell(point);
                if (cell == null)
                    continue;
                
                if (cell.BlocksMovement || cell.BlocksEnvironment)
                    continue;
                
                markedPoints[point.X, point.Y] = true;
                leftPoints.Push(Point.GetPointInDirection(point, Direction.East));
                leftPoints.Push(Point.GetPointInDirection(point, Direction.West));
                leftPoints.Push(Point.GetPointInDirection(point, Direction.North));
                leftPoints.Push(Point.GetPointInDirection(point, Direction.South));
            }

            var result = new List<Point>();
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (markedPoints[x, y])
                        result.Add(new Point(x, y));
                }
            }

            return result.ToArray();
        }

        private static Point FindEmptyRoom(IAreaMap map, bool[,] occupiedCells)
        {
            for (int counter = 0; counter < 5000; counter++)
            {
                var pointX = RandomHelper.GetRandomValue(0, map.Width - 1);
                var pointY = RandomHelper.GetRandomValue(0, map.Height - 1);
                var point = new Point(pointX, pointY);

                if (IsEmptyBlock(map, occupiedCells, point))
                    return point;
            }

            throw new MapGenerationException("Unable to find empty room for squad.");
        }

        private static bool IsEmptyBlock(IAreaMap map, bool[,] occupiedCells, Point position)
        {
            for (int shiftX = 0; shiftX < 3; shiftX++)
            {
                for (int shiftY = 0; shiftY < 3; shiftY++)
                {
                    var x = position.X + shiftX;
                    var y = position.Y + shiftY;

                    var cell = map.TryGetCell(x, y);
                    if (cell == null)
                        return false;

                    if (occupiedCells[x, y])
                        return false;

                    if (cell.BlocksMovement || cell.BlocksEnvironment)
                        return false;
                }
            }

            return true;
        }

        private static int GetSquadsCount(IAreaMap map)
        {
            var square = map.Width * map.Height;
            return (int) Math.Round(square * SquadsCountMultiplier);
        }
    }
}