using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories;
using CodeMagic.Game.Objects;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapGenerators
{
    internal class LabyrinthMapGenerator : IMapAreaGenerator
    {
        private const int SizeSmall = 15;
        private const int SizeMedium = 25;
        private const int SizeBig = 35;

        private const int TorchChance = 10;

        private readonly IDungeonMapObjectFactory mapObjectsFactory;

        public LabyrinthMapGenerator(IDungeonMapObjectFactory mapObjectsFactory)
        {
            this.mapObjectsFactory = mapObjectsFactory;
        }

        public IAreaMap Generate(MapSize size, ItemRareness rareness, bool isLastLevel, out Point playerPosition)
        {
            var mapSize = GetSize(size);

            var labyrinthWidth = (mapSize - 1) / 2;
            var labyrinthHeight = labyrinthWidth;

            var roomsMap = GenerateLabyrinth(labyrinthWidth, labyrinthHeight, out playerPosition);

            playerPosition.X = playerPosition.X * 2 + 1;
            playerPosition.Y = playerPosition.Y * 2 + 1;

            var map = ConvertToAreaMap(roomsMap, mapSize, mapSize);

            var endPosition = GetEndPosition(map, playerPosition);

            map.AddObject(playerPosition, mapObjectsFactory.CreateStairs());
            if (isLastLevel)
            {
                map.AddObject(endPosition, mapObjectsFactory.CreateExitPortal());
            }
            else
            {
                map.AddObject(endPosition, mapObjectsFactory.CreateTrapDoor());
            }

            return map;
        }

        private Point GetEndPosition(IAreaMap map, Point playerPosition)
        {
            const int maxIterations = 100;
            const int minPlayerDistance = 10;

            var iteration = 0;
            while (true)
            {
                if (iteration >= maxIterations)
                    throw new MapGenerationException("Unable to find suitable end point.");

                iteration++;

                var x = RandomHelper.GetRandomValue(0, map.Width - 1);
                var y = RandomHelper.GetRandomValue(0, map.Height - 1);
                var point = new Point(x, y);

                if (Point.GetDistance(playerPosition, point) < minPlayerDistance)
                    continue;

                var cell = map.GetCell(point);

                if (cell.BlocksMovement)
                    continue;

                return point;
            }
        }

        private int GetSize(MapSize size)
        {
            switch (size)
            {
                case MapSize.Small:
                    return SizeSmall;
                case MapSize.Medium:
                    return SizeMedium;
                case MapSize.Big:
                    return SizeBig;
                default:
                    throw new ArgumentException($"Unknown map size: {size}");
            }
        }

        private IAreaMap ConvertToAreaMap(Room[][] roomsMap, int width, int height)
        {
            var map = new AreaMap(() => new GameEnvironment(ConfigurationManager.Current.Physics), width, height, new InsideEnvironmentLightManager());

            var currentMapY = 1;
            foreach (var row in roomsMap)
            {
                var currentMapX = 1;

                foreach (var room in row)
                {
                    currentMapX++;
                    if (room.Walls[Direction.East])
                    {
                        map.AddObject(currentMapX, currentMapY, mapObjectsFactory.CreateWall(TorchChance));
                    }
                    currentMapX++;
                }

                currentMapX = 1;
                currentMapY++;

                foreach (var room in row)
                {
                    if (room.Walls[Direction.South])
                    {
                        map.AddObject(currentMapX, currentMapY, mapObjectsFactory.CreateWall(TorchChance));
                    }
                    currentMapX++;
                    map.AddObject(currentMapX, currentMapY, mapObjectsFactory.CreateWall(TorchChance));
                    currentMapX++;
                }

                currentMapY++;
            }

            for (var y = 0; y < map.Height; y++)
            {
                map.AddObject(0, y, mapObjectsFactory.CreateWall(TorchChance));
            }

            for (var x = 0; x < map.Width; x++)
            {
                map.AddObject(x, 0, mapObjectsFactory.CreateWall(TorchChance));
            }

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    map.AddObject(x, y, mapObjectsFactory.CreateFloor());
                    map.AddObject(x, y, new DungeonRoof());
                }
            }

            return map;
        }

        private Room[][] GenerateLabyrinth(int width, int height, out Point startPosition)
        {
            var map = GetInitialMap(width, height);

            var startingX = RandomHelper.GetRandomValue(1, width - 1);
            var startingY = RandomHelper.GetRandomValue(1, height - 1);
            startPosition = new Point(startingX, startingY);

            GeneratePassages(map, width, height, startingX, startingY);

            return map;
        }

        private void GeneratePassages(Room[][] map, int width, int height, int startX, int startY)
        {
            var currentRoomPos = new Point(startX, startY);
            var room = map[currentRoomPos.Y][currentRoomPos.X];
            room.Visited = true;

            var roomsStack = new Stack<Point>();
            roomsStack.Push(currentRoomPos);
            while (true)
            {
                var neighborDirection = GetUnvisitedNeighborDirection(map, currentRoomPos, width, height);
                if (!neighborDirection.HasValue)
                {
                    if (roomsStack.Count == 0)
                        break;

                    currentRoomPos = roomsStack.Pop();
                    room = map[currentRoomPos.Y][currentRoomPos.X];
                    continue;
                }

                room.Walls[neighborDirection.Value] = false;
                
                currentRoomPos = Point.GetPointInDirection(currentRoomPos, neighborDirection.Value);
                roomsStack.Push(currentRoomPos);
                room = map[currentRoomPos.Y][currentRoomPos.X];
                var negatedDirection = GetNegatedDirection(neighborDirection.Value);
                room.Walls[negatedDirection] = false;
                room.Visited = true;
            }
        }

        private Direction? GetUnvisitedNeighborDirection(Room[][] map, Point location, int width, int height)
        {
            var direction = GetRandomDirection();
            for (var counter = 0; counter < 4; counter++)
            {
                direction = GetNextDirection(direction);
                var newLocation = Point.GetPointInDirection(location, direction);
                if (newLocation.X < 0 || newLocation.X >= width || newLocation.Y < 0 || newLocation.Y >= height)
                    continue;

                var room = map[newLocation.Y][newLocation.X];
                if (room.Visited)
                    continue;

                return direction;
            }

            return null;
        }

        private Direction GetRandomDirection()
        {
            return RandomHelper.GetRandomElement(Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray());
        }

        private Direction GetNextDirection(Direction initial)
        {
            var directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
            var index = directions.IndexOf(initial);
            index++;
            if (index >= directions.Count)
            {
                index = 0;
            }

            return directions[index];
        }

        private Direction GetNegatedDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.South:
                    return Direction.North;
                case Direction.North:
                    return Direction.South;
                case Direction.West:
                    return Direction.East;
                case Direction.East:
                    return Direction.West;
                default:
                    throw new InvalidEnumArgumentException($"Unknown direction {direction}");
            }
        }

        private Room[][] GetInitialMap(int width, int height)
        {
            var map = new Room[height][];
            for (var y = 0; y < height; y++)
            {
                map[y] = new Room[width];
                for (var x = 0; x < width; x++)
                {
                    map[y][x] = new Room();
                }
            }

            return map;
        }

        private class Room
        {
            public Room()
            {
                Walls = new Dictionary<Direction, bool>
                {
                    {Direction.South, true},
                    {Direction.North, true},
                    {Direction.West, true},
                    {Direction.East, true}
                };

                Visited = false;
            }

            public bool Visited { get; set; }

            public Dictionary<Direction, bool> Walls { get; }
        }
    }
}