using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Objects.SolidObjects;

namespace CodeMagic.Core.Game
{
    public class MapGenerator
    {
        private readonly FloorTypes floorType;
        private readonly string wallType;

        public MapGenerator(FloorTypes floorType = FloorTypes.Stone, string wallType = SolidObjectConfiguration.ObjectTypeWallStone)
        {
            this.floorType = floorType;
            this.wallType = wallType;
        }

        public IAreaMap Generate(int width, int height, out Point playerPosition)
        {
            var labyrinthWidth = (width - 1) / 2;
            var labyrinthHeight = (height - 1) / 2;
            var roomsMap = GenerateLabyrinth(labyrinthWidth, labyrinthHeight, out playerPosition);

            playerPosition.X = playerPosition.X * 2 + 1;
            playerPosition.Y = playerPosition.Y * 2 + 1;
            return ConvertToAreaMap(roomsMap, width, height);
        }

        private IAreaMap ConvertToAreaMap(Room[][] roomsMap, int width, int height)
        {
            var map = new AreaMap(width, height);

            File.Delete("C:\\Other\\CodeMagic\\log.txt");
            PrintToFile("C:\\Other\\CodeMagic\\log.txt", roomsMap);

            var currentMapY = 1;
            foreach (var row in roomsMap)
            {
                var currentMapX = 1;

                foreach (var room in row)
                {
                    currentMapX++;
                    if (room.Walls[Direction.Right])
                    {
                        map.GetCell(currentMapX, currentMapY).Objects.Add(CreateWall());
                    }
                    currentMapX++;
                }

                currentMapX = 1;
                currentMapY++;

                foreach (var room in row)
                {
                    if (room.Walls[Direction.Down])
                    {
                        map.GetCell(currentMapX, currentMapY).Objects.Add(CreateWall());
                    }
                    currentMapX++;
                    map.GetCell(currentMapX, currentMapY).Objects.Add(CreateWall());
                    currentMapX++;
                }

                currentMapY++;
            }

            for (var y = 0; y < map.Height; y++)
            {
                map.GetCell(0, y).Objects.Add(CreateWall());
            }

            for (var x = 0; x < map.Width; x++)
            {
                map.GetCell(x, 0).Objects.Add(CreateWall());
            }

            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    map.GetCell(x, y).FloorType = floorType;
                }
            }

            return map;
        }

        private void PrintToFile(string filePath, Room[][] roomsMap)
        {
            File.AppendAllText(filePath, "\r\n");
            foreach (var row in roomsMap)
            {
                foreach (var room in row)
                {
                    File.AppendAllText(filePath, room.Walls[Direction.Up] ? "###" : "# #");
                }
                File.AppendAllText(filePath, "\r\n");
                foreach (var room in row)
                {
                    File.AppendAllText(filePath, room.Walls[Direction.Left] ? "#" : " ");
                    File.AppendAllText(filePath, " ");
                    File.AppendAllText(filePath, room.Walls[Direction.Right] ? "#" : " ");
                }
                File.AppendAllText(filePath, "\r\n");
                foreach (var room in row)
                {
                    File.AppendAllText(filePath, room.Walls[Direction.Down] ? "###" : "# #");
                }
                File.AppendAllText(filePath, "\r\n");
            }
        }

        private SolidObject CreateWall()
        {
            return new SolidObject(new SolidObjectConfiguration
            {
                Name = "Wall",
                Type = wallType
            });
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
                
                currentRoomPos = Point.GetAdjustedPoint(currentRoomPos, neighborDirection.Value);
                roomsStack.Push(currentRoomPos);
                room = map[currentRoomPos.Y][currentRoomPos.X];
                var negatedDirection = GetNegatedDirection(neighborDirection.Value);
                room.Walls[negatedDirection] = false;
                room.Visited = true;
            }
        }

        private Direction? GetUnvisitedNeighborDirection(Room[][] map, Point location, int width, int height)
        {
            var directionValue = RandomHelper.GetRandomValue(0, 3);

            for (var counter = 0; counter < 4; counter++)
            {
                directionValue += counter;
                if (directionValue > 3)
                    directionValue = 0;

                var direction = (Direction) directionValue;
                var newLocation = Point.GetAdjustedPoint(location, direction);
                if (newLocation.X < 0 || newLocation.X >= width || newLocation.Y < 0 || newLocation.Y >= height)
                    continue;

                var room = map[newLocation.Y][newLocation.X];
                if (room.Visited)
                    continue;

                return direction;
            }

            return null;
        }

        private Direction GetNegatedDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
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
                    {Direction.Down, true},
                    {Direction.Up, true},
                    {Direction.Left, true},
                    {Direction.Right, true}
                };

                Visited = false;
            }

            public bool Visited { get; set; }

            public Dictionary<Direction, bool> Walls { get; }
        }
    }
}