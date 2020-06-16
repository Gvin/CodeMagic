using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapGenerators
{
    internal class CaveDungeonMapGenerator : IMapAreaGenerator
    {
        private readonly IMapObjectFactory mapObjectsFactory;

        public CaveDungeonMapGenerator(IMapObjectFactory mapObjectsFactory)
        {
            this.mapObjectsFactory = mapObjectsFactory;
        }

        public IAreaMap Generate(int level, MapSize size, out Point playerPosition)
        {
            var generator = ConfigureMapHandler(size);
            generator.RandomFillMap();
            generator.MakeCaverns();

            var simplifiedMap = SimplifyMap(generator.Map, generator.MapWidth, generator.MapHeight);
            var height = simplifiedMap.Length;
            var width = simplifiedMap[0].Length;

            var map = ConvertMap(level, simplifiedMap, width, height);

            playerPosition = FindPlayerPosition(map);
            map.AddObject(playerPosition, mapObjectsFactory.CreateStairs());

            var trapDoorPosition = FindTrapDoorPosition(map, playerPosition);
            map.AddObject(trapDoorPosition, mapObjectsFactory.CreateTrapDoor());

            return map;
        }

        private Point FindTrapDoorPosition(IAreaMap map, Point playerPosition)
        {
            const int maxIterations = 100;
            var iteration = 0;
            var minDistanceToPlayer = map.Width / 4;

            while (iteration < maxIterations)
            {
                iteration++;

                var x = RandomHelper.GetRandomValue(1, map.Width - 2);
                var y = RandomHelper.GetRandomValue(1, map.Height - 2);
                var position = new Point(x, y);

                if (Point.GetDistance(playerPosition, position) < minDistanceToPlayer)
                    continue;

                if (map.GetCell(position).BlocksMovement)
                    continue;

                return position;
            }
            throw new MapGenerationException("Unable to find trap door position");
        }

        private MapHandler ConfigureMapHandler(MapSize size)
        {
            const int sizeSmall = 25;
            const int sizeMedium = 35;
            const int sizeBig = 45;

            switch (size)
            {
                case MapSize.Small:
                    return new MapHandler(sizeSmall, sizeSmall, new int[sizeSmall, sizeSmall]);
                case MapSize.Medium:
                    return new MapHandler(sizeMedium, sizeMedium, new int[sizeMedium, sizeMedium]);
                case MapSize.Big:
                    return new MapHandler(sizeBig, sizeBig, new int[sizeBig, sizeBig]);
                default:
                    throw new ArgumentException($"Unknown map size {size}");
            }
        }

        private int[][] SimplifyMap(int[,] map, int width, int height)
        {
            var arrayMap = new List<int[]>();

            for (int y = 0; y < height; y++)
            {
                var line = new List<int>();
                for (int x = 0; x < width; x++)
                {
                    line.Add(map[x, y]);
                }

                arrayMap.Add(line.ToArray());
            }

            var clearedMap = RemoveUnnecessaryCells(arrayMap.ToArray());
            var result = new List<List<int>>();
            result.Add(GetIndestructibleLine(clearedMap[0].Length + 2));
            foreach (var line in clearedMap)
            {
                var lineData = new List<int>();
                lineData.Add(MapHandler.IndestructibleCell);
                lineData.AddRange(line);
                lineData.Add(MapHandler.IndestructibleCell);
                result.Add(lineData);
            }
            result.Add(GetIndestructibleLine(clearedMap[0].Length + 2));

            return result.Select(line => line.ToArray()).ToArray();
        }

        private List<int> GetIndestructibleLine(int width)
        {
            var result = new List<int>();

            for (int x = 0; x < width; x++)
            {
                result.Add(MapHandler.IndestructibleCell);
            }

            return result;
        }

        private int[][] RemoveUnnecessaryCells(int[][] map)
        {
            var linesRemoved = RemoveUnnecessaryLines(map);
            var turnedMap = TurnMap(linesRemoved);
            var colsRemoved = RemoveUnnecessaryLines(turnedMap);
            return TurnMap(colsRemoved);
        }

        private int[][] RemoveUnnecessaryLines(int[][] map)
        {
            var dynamicMap = map.ToList();
            for (int y = 0; y < map.Length - 1; y++) // For all lines except for the last one
            {
                var line = map[y];
                var nextLine = map[y + 1];

                if (line.All(cell => cell == MapHandler.FilledCell) &&
                    nextLine.All(cell => cell == MapHandler.FilledCell)) // If 2 cells in a row are filled
                {
                    dynamicMap.Remove(line);
                }
            }

            return dynamicMap.ToArray();
        }

        private int[][] TurnMap(int[][] map)
        {
            var turnedMap = new List<int[]>();
            for (var x = 0; x < map[0].Length; x++)
            {
                var turnedLine = new List<int>();
                for (var y = 0; y < map.Length; y++)
                {
                    turnedLine.Add(map[y][x]);
                }
                turnedMap.Add(turnedLine.ToArray());
            }

            return turnedMap.ToArray();
        }

        private Point FindPlayerPosition(IAreaMap map)
        {
            const int maxIterations = 100;
            var iteration = 0;
            while (iteration < maxIterations)
            {
                var x = RandomHelper.GetRandomValue(1, map.Width - 1);
                var y = RandomHelper.GetRandomValue(1, map.Height - 1);

                var cell = map.GetCell(x, y);
                if (!cell.BlocksMovement && !cell.BlocksEnvironment)
                    return new Point(x, y);

                iteration++;
            }

            throw new ApplicationException("Unable to find player position.");
        }

        private IAreaMap ConvertMap(int level, int[][] map, int width, int height)
        {
            var result = new AreaMap(level, () => new AreaMapCell(new GameEnvironment()), width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result.AddObject(x, y, mapObjectsFactory.CreateFloor());

                    if (map[y][x] == MapHandler.FilledCell || map[y][x] == MapHandler.IndestructibleCell)
                    {
                        var wall = mapObjectsFactory.CreateWall();
                        result.AddObject(x, y, wall);
                    }
                }
            }

            return result;
        }

        private class MapHandler
        {
            public const int FilledCell = 1;
            public const int EmptyCell = 0;
            public const int IndestructibleCell = 2;

            private Random rand = RandomHelper.GetRandomGenerator();


            public int MapWidth { get; set; }
            public int MapHeight { get; set; }
            public int PercentAreWalls { get; set; }

            public int[,] Map;

            public void MakeCaverns()
            {
                // By initilizing column in the outter loop, its only created ONCE
                for (int column = 0, row = 0; row <= MapHeight - 1; row++)
                {
                    for (column = 0; column <= MapWidth - 1; column++)
                    {
                        Map[column, row] = PlaceWallLogic(column, row);
                    }
                }
            }

            public int PlaceWallLogic(int x, int y)
            {
                int numWalls = GetAdjacentWalls(x, y, 1, 1);


                if (Map[x, y] == 1)
                {
                    if (numWalls >= 4)
                    {
                        return 1;
                    }
                    if (numWalls < 2)
                    {
                        return 0;
                    }

                }
                else
                {
                    if (numWalls >= 5)
                    {
                        return 1;
                    }
                }
                return 0;
            }

            public int GetAdjacentWalls(int x, int y, int scopeX, int scopeY)
            {
                int startX = x - scopeX;
                int startY = y - scopeY;
                int endX = x + scopeX;
                int endY = y + scopeY;

                int iX = startX;
                int iY = startY;

                int wallCounter = 0;

                for (iY = startY; iY <= endY; iY++)
                {
                    for (iX = startX; iX <= endX; iX++)
                    {
                        if (!(iX == x && iY == y))
                        {
                            if (IsWall(iX, iY))
                            {
                                wallCounter += 1;
                            }
                        }
                    }
                }
                return wallCounter;
            }

            bool IsWall(int x, int y)
            {
                // Consider out-of-bound a wall
                if (IsOutOfBounds(x, y))
                {
                    return true;
                }

                if (Map[x, y] == 1)
                {
                    return true;
                }

                if (Map[x, y] == 0)
                {
                    return false;
                }
                return false;
            }

            bool IsOutOfBounds(int x, int y)
            {
                if (x < 0 || y < 0)
                {
                    return true;
                }
                else if (x > MapWidth - 1 || y > MapHeight - 1)
                {
                    return true;
                }
                return false;
            }

            

            string MapToString()
            {
                string returnString = string.Join(" ", // Seperator between each element
                                                  "Width:",
                                                  MapWidth.ToString(),
                                                  "\tHeight:",
                                                  MapHeight.ToString(),
                                                  "\t% Walls:",
                                                  PercentAreWalls.ToString(),
                                                  Environment.NewLine
                                                 );

                List<string> mapSymbols = new List<string>();
                mapSymbols.Add(".");
                mapSymbols.Add("#");
                mapSymbols.Add("+");

                for (int column = 0, row = 0; row < MapHeight; row++)
                {
                    for (column = 0; column < MapWidth; column++)
                    {
                        returnString += mapSymbols[Map[column, row]];
                    }
                    returnString += Environment.NewLine;
                }
                return returnString;
            }

            public MapHandler(int mapWidth, int mapHeight, int[,] map, int percentWalls = 40)
            {
                this.MapWidth = mapWidth;
                this.MapHeight = mapHeight;
                this.PercentAreWalls = percentWalls;
                this.Map = map;
            }

            public MapHandler()
            {
                MapWidth = 40;
                MapHeight = 21;
                PercentAreWalls = 40;

                Map = new int[MapWidth, MapHeight];

                RandomFillMap();
            }

            public void BlankMap()
            {
                for (int column = 0, row = 0; row < MapHeight; row++)
                {
                    for (column = 0; column < MapWidth; column++)
                    {
                        Map[column, row] = 0;
                    }
                }
            }

            public void RandomFillMap()
            {
                // New, empty map
                Map = new int[MapWidth, MapHeight];

                int mapMiddle = 0; // Temp variable
                for (int column = 0, row = 0; row < MapHeight; row++)
                {
                    for (column = 0; column < MapWidth; column++)
                    {
                        // If coordinants lie on the the edge of the map (creates a border)
                        if (column == 0)
                        {
                            Map[column, row] = 1;
                        }
                        else if (row == 0)
                        {
                            Map[column, row] = 1;
                        }
                        else if (column == MapWidth - 1)
                        {
                            Map[column, row] = 1;
                        }
                        else if (row == MapHeight - 1)
                        {
                            Map[column, row] = 1;
                        }
                        // Else, fill with a wall a random percent of the time
                        else
                        {
                            mapMiddle = (MapHeight / 2);

                            if (row == mapMiddle)
                            {
                                Map[column, row] = 0;
                            }
                            else
                            {
                                Map[column, row] = RandomPercent(PercentAreWalls);
                            }
                        }
                    }
                }
            }

            int RandomPercent(int percent)
            {
                if (percent >= rand.Next(1, 101))
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}