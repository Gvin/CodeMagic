using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories;
using CodeMagic.Game.MapGeneration.Dungeon.MonstersGenerators;
using CodeMagic.Game.MapGeneration.Dungeon.ObjectsGenerators;
using Point = System.Drawing.Point;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapGenerators
{
    internal class DungeonRoomsMapGenerator : IMapAreaGenerator
    {
        private const int TorchChance = 5;
        private const int MaxBuildRetries = 10;

        private readonly IMapObjectFactory mapObjectsFactory;
        private readonly IObjectsGenerator objectsGenerator;
        private readonly IMonstersGenerator monstersGenerator;

        public DungeonRoomsMapGenerator(IMapObjectFactory mapObjectsFactory,
            IObjectsGenerator objectsGenerator,
            IMonstersGenerator monstersGenerator)
        {
            this.mapObjectsFactory = mapObjectsFactory;
            this.objectsGenerator = objectsGenerator;
            this.monstersGenerator = monstersGenerator;
        }

        public IAreaMap Generate(int level, MapSize size, out Core.Game.Point playerPosition)
        {
            var builder = new MapBuilder(1, 1);
            switch (size)
            {
                case MapSize.Small:
                    ConfigureSmallMap(builder);
                    break;
                case MapSize.Medium:
                    ConfigureMediumMap(builder);
                    break;
                case MapSize.Big:
                    ConfigureBigMap(builder);
                    break;
                default:
                    throw new ArgumentException($"Unknown map size: {size}");
            }

            var mapBuilt = BuildMap(builder);
            if (!mapBuilt)
                throw new MapGenerationException("Unable to build dungeon map.");

            var simplifiedMap = SimplifyMap(builder.Map, builder.MapSize.Width, builder.MapSize.Height);
            var height = simplifiedMap.Length;
            var width = simplifiedMap[0].Length;

            var map = ConvertMap(level, simplifiedMap, width, height);
            playerPosition = FindPlayerPosition(map);

            if (level > 1)
            {
                map.AddObject(playerPosition, mapObjectsFactory.CreateStairs());
            }

            objectsGenerator.GenerateObjects(map);
            monstersGenerator.GenerateMonsters(map, playerPosition);

            return map;
        }

        private bool BuildMap(MapBuilder builder)
        {
            for (var counter = 0; counter < MaxBuildRetries; counter++)
            {
                try
                {
                    var mapBuilt = builder.BuildOneStartRoom();
                    if (mapBuilt)
                        return true;
                }
                catch (MapGenerationException)
                {
                    // Do nothing.
                }
            }

            return false;
        }

        private Core.Game.Point FindPlayerPosition(IAreaMap map)
        {
            const int maxIterations = 100;
            var iteration = 0;
            while (iteration < maxIterations)
            {
                var x = RandomHelper.GetRandomValue(1, map.Width - 1);
                var y = RandomHelper.GetRandomValue(1, map.Height - 1);

                var cell = map.GetCell(x, y);
                if (!cell.BlocksMovement && !cell.BlocksEnvironment)
                    return new Core.Game.Point(x, y);

                iteration++;
            }

            throw new ApplicationException("Unable to find player position.");
        }

        private IAreaMap ConvertMap(int level, int[][] map, int width, int height)
        {
            var result = new AreaMap(level, () => new GameEnvironment(), width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result.AddObject(x, y, mapObjectsFactory.CreateFloor());

                    if (map[y][x] == MapBuilder.FilledCell)
                    {
                        var wall = mapObjectsFactory.CreateWall(TorchChance);
                        result.AddObject(x, y, wall);
                    }
                    else if (map[y][x] == MapBuilder.DoorCell)
                    {
                        var door = mapObjectsFactory.CreateDoor();
                        result.AddObject(x, y, door);
                    }
                    else if (map[y][x] == MapBuilder.TrapDoorCell)
                    {
                        var trapDoor = mapObjectsFactory.CreateTrapDoor();
                        result.AddObject(x, y, trapDoor);
                    }
                }
            }

            return result;
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

            return RemoveUnnecessaryCells(arrayMap.ToArray());
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

                if (line.All(cell => cell == MapBuilder.FilledCell) &&
                    nextLine.All(cell => cell == MapBuilder.FilledCell)) // If 2 cells in a row are filled
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

        private void ConfigureMediumMap(MapBuilder builder)
        {
            builder.MapSize = new Size(55, 55);

            builder.RoomSizeMin = new Size(3, 3);
            builder.RoomSizeMax = new Size(8, 8);
            builder.RoomDistance = 2;
            builder.CorridorSpace = 10;
            builder.CorridorDistance = 5;
            builder.CorridorLengthMax = 7;
            builder.CorridorLengthMin = 3;
            builder.CorridorMaxTurns = 3;
            builder.BreakOut = 1000;
        }

        private void ConfigureSmallMap(MapBuilder builder)
        {
            builder.MapSize = new Size(40, 40);

            builder.RoomSizeMin = new Size(3, 3);
            builder.RoomSizeMax = new Size(6, 6);
            builder.RoomDistance = 1;
            builder.CorridorSpace = 10;
            builder.CorridorDistance = 5;
            builder.CorridorLengthMax = 3;
            builder.CorridorLengthMin = 1;
            builder.CorridorMaxTurns = 3;
            builder.BreakOut = 1000;
        }

        private void ConfigureBigMap(MapBuilder builder)
        {
            builder.MapSize = new Size(70, 70);

            builder.RoomSizeMin = new Size(3, 3);
            builder.RoomSizeMax = new Size(10, 10);
            builder.RoomDistance = 2;
            builder.CorridorSpace = 10;
            builder.CorridorDistance = 5;
            builder.CorridorLengthMax = 12;
            builder.CorridorLengthMin = 3;
            builder.CorridorMaxTurns = 5;
            builder.BreakOut = 1000;
        }

        /// <summary>
        /// Black magic code which generates dungeon.
        /// </summary>
        private class MapBuilder
        {
            public int[,] Map;

            /// <summary>
            /// Built rooms stored here
            /// </summary>
            private List<Rectangle> rctBuiltRooms;

            /// <summary>
            /// Built corridors stored here
            /// </summary>
            private List<Point> lBuiltCorridors;

            /// <summary>
            /// Corridor to be built stored here
            /// </summary>
            private List<Point> lPotentialCorridor;

            /// <summary>
            /// Room to be built stored here
            /// </summary>
            private Rectangle rctCurrentRoom;


            #region builder public properties

            /// <summary>
            /// Minimum Room Size
            /// </summary>
            public Size RoomSizeMin { get; set; }

            /// <summary>
            /// Maximum Room Size
            /// </summary>
            public Size RoomSizeMax { get; set; }

            /// <summary>
            /// Rooms to build
            /// </summary>
            public int MaxRoomsCount { get; set; }

            /// <summary>
            /// Minimum distance between rooms
            /// </summary>
            public int RoomDistance { get; set; }

            /// <summary>
            /// Minimum distance of room from existing corridors
            /// </summary>
            public int CorridorDistance { get; set; }

            /// <summary>
            /// Minimum corridor length
            /// </summary>
            public int CorridorLengthMin { get; set; }

            /// <summary>
            /// Maximum corridor length
            /// </summary>
            public int CorridorLengthMax { get; set; }

            /// <summary>
            /// Maximum turns
            /// </summary>
            public int CorridorMaxTurns { get; set; }

            /// <summary>
            /// The distance a corridor has to be away from a closed cell for it to be built
            /// </summary>
            public int CorridorSpace { get; set; }


            /// <summary>
            /// Probability of building a corridor from a room or corridor. Greater than value = room
            /// </summary>
            public int BuildProb { get; set; }

            /// <summary>
            /// Map Size
            /// </summary>
            public Size MapSize { get; set; }

            /// <summary>
            /// Break Out Limit
            /// </summary>
            public int BreakOut { get; set; }



            #endregion

            /// <summary>
            /// describes the outcome of the corridor building operation
            /// </summary>
            private enum CorridorItemHit
            {
                /// <summary>
                /// invalid Point generated
                /// </summary>
                Invalid,

                /// <summary>
                /// corridor hit self
                /// </summary>
                Self,

                /// <summary>
                /// hit a built corridor
                /// </summary>
                ExistingCorridor,

                /// <summary>
                /// corridor hit origin room
                /// </summary>
                OriginRoom,

                /// <summary>
                /// corridor hit existing room
                /// </summary>
                ExistingRoom,

                /// <summary>
                /// corridor built without problem
                /// </summary>
                Completed,

                TooClose,

                /// <summary>
                /// Point OK
                /// </summary>
                Ok
            }

            private static readonly Point[] DirectionsStraight =
            {
                new Point(0, -1), //n
                new Point(0, 1),  //s
                new Point(1, 0),  //w
                new Point(-1, 0)  //e
            };

            public const int FilledCell = 1;
            public const int EmptyCell = 0;
            public const int DoorCell = 3;
            public const int TrapDoorCell = 4;

            private readonly Random random;

            public MapBuilder(int x, int y)
            {
                random = RandomHelper.GetRandomGenerator();

                MapSize = new Size(x, y);
                Map = new int[MapSize.Width, MapSize.Height];
                CorridorMaxTurns = 5;
                RoomSizeMin = new Size(3, 3);
                RoomSizeMax = new Size(15, 15);
                CorridorLengthMin = 3;
                CorridorLengthMax = 15;
                MaxRoomsCount = 15;

                RoomDistance = 5;
                CorridorDistance = 2;
                CorridorSpace = 2;

                BuildProb = 50;
                BreakOut = 250;
            }

            /// <summary>
            /// Initialise everything
            /// </summary>
            private void Clear()
            {
                lPotentialCorridor = new List<Point>();
                rctBuiltRooms = new List<Rectangle>();
                lBuiltCorridors = new List<Point>();

                Map = new int[MapSize.Width, MapSize.Height];
                for (int x = 0; x < MapSize.Width; x++)
                    for (int y = 0; y < MapSize.Width; y++)
                        Map[x, y] = FilledCell;
            }

            #region build methods()

            /// <summary>
            /// Randomly choose a room and attempt to build a corridor terminated by
            /// a room off it, and repeat until MaxRooms has been reached. The map
            /// is started of by placing a room in approximately the centre of the map
            /// using the method PlaceStartRoom()
            /// </summary>
            /// <returns>Bool indicating if the map was built, i.e. the property BreakOut was not
            /// exceed</returns>
            public bool BuildOneStartRoom()
            {
                int loopctr = 0;

                CorridorItemHit CorBuildOutcome;
                Point Location = new Point();
                Point Direction = new Point();

                Clear();

                PlaceStartRoom();

                //attempt to build the required number of rooms
                while (rctBuiltRooms.Count < MaxRoomsCount)
                {
                    var lastRoom = rctBuiltRooms.Count == MaxRoomsCount - 1;
                    if (loopctr++ > BreakOut)//bail out if this value is exceeded
                        return false;

                    if (CorridorGetStart(out Location, out Direction))
                    {

                        CorBuildOutcome = CorridorMakeStraight(
                            ref Location, 
                            ref Direction, 
                            random.Next(1, CorridorMaxTurns), 
                            random.Next(0, 100) > 50);

                        switch (CorBuildOutcome)
                        {
                            case CorridorItemHit.ExistingRoom:
                            case CorridorItemHit.ExistingCorridor:
                            case CorridorItemHit.Self:
                                CorridorBuild();
                                break;

                            case CorridorItemHit.Completed:
                                if (RoomAttemptBuildOnCorridor(Direction))
                                {
                                    CorridorBuild();
                                    BuildRoom(lastRoom);
                                }
                                break;
                        }
                    }
                }

                return true;
            }

            /// <summary>
            /// Randomly choose a room and attempt to build a corridor terminated by
            /// a room off it, and repeat until MaxRooms has been reached. The map
            /// is started of by placing two rooms on opposite sides of the map and joins
            /// them with a long corridor, using the method PlaceStartRooms()
            /// </summary>
            /// <returns>Bool indicating if the map was built, i.e. the property BreakOut was not
            /// exceed</returns>
            public bool BuildConnectedStartRooms()
            {
                int loopctr = 0;

                CorridorItemHit CorBuildOutcome;
                Point Location = new Point();
                Point Direction = new Point();

                Clear();

                PlaceStartRooms();

                //attempt to build the required number of rooms
                while (rctBuiltRooms.Count < MaxRoomsCount)
                {
                    var lastRoom = rctBuiltRooms.Count == MaxRoomsCount - 1;
                    if (loopctr++ > BreakOut)//bail out if this value is exceeded
                        return false;

                    if (CorridorGetStart(out Location, out Direction))
                    {

                        CorBuildOutcome = CorridorMakeStraight(ref Location, ref Direction, random.Next(1, CorridorMaxTurns)
                            , random.Next(0, 100) > 50);

                        switch (CorBuildOutcome)
                        {
                            case CorridorItemHit.ExistingRoom:
                            case CorridorItemHit.ExistingCorridor:
                            case CorridorItemHit.Self:
                                CorridorBuild();
                                break;

                            case CorridorItemHit.Completed:
                                if (RoomAttemptBuildOnCorridor(Direction))
                                {
                                    CorridorBuild();
                                    BuildRoom(lastRoom);
                                }
                                break;
                        }
                    }
                }

                return true;

            }

            #endregion


            #region room utilities

            /// <summary>
            /// Place a random sized room in the middle of the map
            /// </summary>
            private void PlaceStartRoom()
            {
                rctCurrentRoom = new Rectangle()
                {
                    Width = random.Next(RoomSizeMin.Width, RoomSizeMax.Width),
                    Height = random.Next(RoomSizeMin.Height, RoomSizeMax.Height)
                };
                rctCurrentRoom.X = MapSize.Width / 2;
                rctCurrentRoom.Y = MapSize.Height / 2;
                BuildRoom(false);
            }


            /// <summary>
            /// Place a start room anywhere on the map
            /// </summary>
            private void PlaceStartRooms()
            {
                Point startdirection;
                bool connection = false;
                Point Location = new Point();
                Point Direction = new Point();
                CorridorItemHit CorBuildOutcome;

                while (!connection)
                {
                    Clear();
                    startdirection = GetDirection(new Point());

                    //place a room on the top and bottom
                    if (startdirection.X == 0)
                    {
                        //room at the top of the map
                        rctCurrentRoom = new Rectangle
                        {
                            X = random.Next(0, MapSize.Width - rctCurrentRoom.Width),
                            Y = 1,
                            Width = random.Next(RoomSizeMin.Width, RoomSizeMax.Width),
                            Height = random.Next(RoomSizeMin.Height, RoomSizeMax.Height)
                        };
                        BuildRoom(false);

                        //at the bottom of the map
                        rctCurrentRoom = new Rectangle
                        {
                            X = random.Next(0, MapSize.Width - rctCurrentRoom.Width),
                            Y = MapSize.Height - rctCurrentRoom.Height - 1,
                            Width = random.Next(RoomSizeMin.Width, RoomSizeMax.Width),
                            Height = random.Next(RoomSizeMin.Height, RoomSizeMax.Height)
                        };
                        BuildRoom(false);
                    }
                    else//place a room on the east and west side
                    {
                        //west side of room
                        rctCurrentRoom = new Rectangle
                        {
                            X = 1,
                            Y = random.Next(0, MapSize.Height - rctCurrentRoom.Height),
                            Width = random.Next(RoomSizeMin.Width, RoomSizeMax.Width),
                            Height = random.Next(RoomSizeMin.Height, RoomSizeMax.Height)
                        };
                        BuildRoom(false);

                        rctCurrentRoom = new Rectangle
                        {
                            X = MapSize.Width - rctCurrentRoom.Width - 2,
                            Y = random.Next(0, MapSize.Height - rctCurrentRoom.Height),
                            Width = random.Next(RoomSizeMin.Width, RoomSizeMax.Width),
                            Height = random.Next(RoomSizeMin.Height, RoomSizeMax.Height)
                        };
                        BuildRoom(false);

                    }

                    if (CorridorGetStart(out Location, out Direction))
                    {
                        CorBuildOutcome = CorridorMakeStraight(ref Location, ref Direction, 100, true);

                        switch (CorBuildOutcome)
                        {
                            case CorridorItemHit.ExistingRoom:
                                CorridorBuild();
                                connection = true;
                                break;
                        }
                    }
                }
            }

            /// <summary>
            /// Make a room off the last Point of Corridor, using
            /// CorridorDirection as an indicator of how to offset the room.
            /// The potential room is stored in Room.
            /// </summary>
            private bool RoomAttemptBuildOnCorridor(Point pDirection)
            {
                rctCurrentRoom = new Rectangle()
                {
                    Width = random.Next(RoomSizeMin.Width, RoomSizeMax.Width),
                    Height = random.Next(RoomSizeMin.Height, RoomSizeMax.Height)
                };

                //start building room from this Point
                Point lc = lPotentialCorridor.Last();

                if (pDirection.X == 0) //north/south direction
                {
                    rctCurrentRoom.X = random.Next(lc.X - rctCurrentRoom.Width + 1, lc.X);

                    if (pDirection.Y == 1)
                        rctCurrentRoom.Y = lc.Y + 1;//south
                    else
                        rctCurrentRoom.Y = lc.Y - rctCurrentRoom.Height - 1;//north


                }
                else if (pDirection.Y == 0)//east / west direction
                {
                    rctCurrentRoom.Y = random.Next(lc.Y - rctCurrentRoom.Height + 1, lc.Y);

                    if (pDirection.X == -1)//west
                        rctCurrentRoom.X = lc.X - rctCurrentRoom.Width;
                    else
                        rctCurrentRoom.X = lc.X + 1;//east
                }

                return VerifyRoom();
            }


            /// <summary>
            /// Randomly get a Point on the edge of a randomly selected room
            /// </summary>
            /// <param name="pLocation">Out: Location of Point on room edge</param>
            /// <param name="pDirection">Out: Direction of Point</param>
            /// <returns>If Location is legal</returns>
            private void RoomGetEdge(out Point pLocation, out Point pDirection)
            {

                rctCurrentRoom = rctBuiltRooms[random.Next(0, rctBuiltRooms.Count)];

                //pick a random Point within a room
                //the +1 / -1 on the values are to stop a corner from being chosen
                pLocation = new Point(
                    random.Next(rctCurrentRoom.Left + 1, rctCurrentRoom.Right - 1), 
                    random.Next(rctCurrentRoom.Top + 1, rctCurrentRoom.Bottom - 1));


                //get a random direction
                pDirection = DirectionsStraight[random.Next(0, DirectionsStraight.GetLength(0))];

                do
                {
                    //move in that direction
                    pLocation.Offset(pDirection);

                    if (!ValidatePoint(pLocation.X, pLocation.Y))
                        return;

                    //until we meet an empty cell
                } while (!IsFilledPoint(pLocation.X, pLocation.Y));

            }

            #endregion

            #region corridor utitlies

            /// <summary>
            /// Randomly get a Point on an existing corridor
            /// </summary>
            /// <param name="pLocation">Out: location of Point</param>
            /// <param name="pDirection">Out: direction of Point</param>
            /// <returns>Bool indicating success</returns>
            private void CorridorGetEdge(out Point pLocation, out Point pDirection)
            {
                List<Point> validDirections = new List<Point>();

                const int maxIterations = 100;
                var iteration = 0;

                do
                {
                    iteration++;

                    //the modifiers below prevent the first of last Point being chosen
                    pLocation = lBuiltCorridors[random.Next(1, lBuiltCorridors.Count - 1)];

                    //attempt to locate all the empty map Points around the location
                    //using the directions to offset the randomly chosen Point
                    foreach (Point p in DirectionsStraight)
                        if (ValidatePoint(pLocation.X + p.X, pLocation.Y + p.Y))
                            if (IsFilledPoint(pLocation.X + p.X, pLocation.Y + p.Y))
                                validDirections.Add(p);

                    if (iteration > maxIterations)
                        throw new MapGenerationException();

                } while (validDirections.Count == 0);

                pDirection = validDirections[random.Next(0, validDirections.Count)];
                pLocation.Offset(pDirection);

            }

            /// <summary>
            /// Build the contents of lPotentialCorridor, adding it's Points to the builtCorridors
            /// list then empty
            /// </summary>
            private void CorridorBuild()
            {
                foreach (Point p in lPotentialCorridor)
                {
                    SetPoint(p.X, p.Y, EmptyCell);
                    lBuiltCorridors.Add(p);
                }

                var startPoint = lPotentialCorridor.First();
                var endPoint = lPotentialCorridor.Last();
                SetPoint(startPoint.X, startPoint.Y, DoorCell);
                SetPoint(endPoint.X, endPoint.Y, DoorCell);

                lPotentialCorridor.Clear();
            }

            /// <summary>
            /// Get a starting Point for a corridor, randomly choosing between a room and a corridor.
            /// </summary>
            /// <param name="pLocation">Out: pLocation of Point</param>
            /// <param name="pDirection">Out: pDirection of Point</param>
            /// <returns>Bool indicating if location found is OK</returns>
            private bool CorridorGetStart(out Point pLocation, out Point pDirection)
            {
                rctCurrentRoom = new Rectangle();
                lPotentialCorridor = new List<Point>();

                if (lBuiltCorridors.Count > 0)
                {
                    if (random.Next(0, 100) >= BuildProb)
                        RoomGetEdge(out pLocation, out pDirection);
                    else
                        CorridorGetEdge(out pLocation, out pDirection);
                }
                else//no corridors present, so build off a room
                    RoomGetEdge(out pLocation, out pDirection);

                //finally check the Point we've found
                return CorridorPointTest(pLocation, pDirection) == CorridorItemHit.Ok;

            }

            /// <summary>
            /// Attempt to make a corridor, storing it in the lPotentialCorridor list
            /// </summary>
            /// <param name="pStart">Start Point of corridor</param>
            /// <param name="pTurns">Number of turns to make</param>
            private CorridorItemHit CorridorMakeStraight(ref Point pStart, ref Point pDirection, int pTurns, bool pPreventBackTracking)
            {

                lPotentialCorridor = new List<Point> {pStart};

                int corridorlength;
                Point startdirection = new Point(pDirection.X, pDirection.Y);
                CorridorItemHit outcome;

                while (pTurns > 0)
                {
                    pTurns--;

                    corridorlength = random.Next(CorridorLengthMin, CorridorLengthMax);
                    //build corridor
                    while (corridorlength > 0)
                    {
                        corridorlength--;

                        //make a Point and offset it
                        pStart.Offset(pDirection);

                        outcome = CorridorPointTest(pStart, pDirection);
                        if (outcome != CorridorItemHit.Ok)
                            return outcome;
                        else
                            lPotentialCorridor.Add(pStart);
                    }

                    if (pTurns > 1)
                        if (!pPreventBackTracking)
                            pDirection = GetDirection(pDirection);
                        else
                            pDirection = GetDirection(pDirection, startdirection);
                }

                return CorridorItemHit.Completed;
            }

            /// <summary>
            /// Test the provided Point to see if it has empty cells on either side
            /// of it. This is to stop corridors being built adjacent to a room.
            /// </summary>
            /// <param name="pPoint">Point to test</param>
            /// <param name="pDirection">Direction it is moving in</param>
            /// <returns></returns>
            private CorridorItemHit CorridorPointTest(Point pPoint, Point pDirection)
            {

                if (!ValidatePoint(pPoint.X, pPoint.Y))//invalid Point hit, exit
                    return CorridorItemHit.Invalid;
                if (lBuiltCorridors.Contains(pPoint))//in an existing corridor
                    return CorridorItemHit.ExistingCorridor;
                if (lPotentialCorridor.Contains(pPoint))//hit self
                    return CorridorItemHit.Self;
                if (rctCurrentRoom != null && rctCurrentRoom.Contains(pPoint))//the corridors origin room has been reached, exit
                    return CorridorItemHit.OriginRoom;

                //is Point in a room
                foreach (Rectangle r in rctBuiltRooms)
                    if (r.Contains(pPoint))
                        return CorridorItemHit.ExistingRoom;


                //using the property corridor space, check that number of cells on
                //either side of the Point are empty
                foreach (int r in Enumerable.Range(-CorridorSpace, 2 * CorridorSpace + 1).ToList())
                {
                    if (pDirection.X == 0)//north or south
                    {
                        if (ValidatePoint(pPoint.X + r, pPoint.Y))
                            if (!IsFilledPoint(pPoint.X + r, pPoint.Y))
                                return CorridorItemHit.TooClose;
                    }
                    else if (pDirection.Y == 0)//east west
                    {
                        if (ValidatePoint(pPoint.X, pPoint.Y + r))
                            if (!IsFilledPoint(pPoint.X, pPoint.Y + r))
                                return CorridorItemHit.TooClose;
                    }

                }

                return CorridorItemHit.Ok;
            }


            #endregion

            #region direction methods

            /// <summary>
            /// Get a random direction, excluding the opposite of the provided direction to
            /// prevent a corridor going back on it's Build
            /// </summary>
            /// <param name="pDir">Current direction</param>
            /// <returns></returns>
            private Point GetDirection(Point pDir)
            {
                Point newDir;
                do
                {
                    newDir = DirectionsStraight[random.Next(0, DirectionsStraight.GetLength(0))];
                } while (ReverseDirection(newDir) == pDir);

                return newDir;
            }

            /// <summary>
            /// Get a random direction, excluding the provided directions and the opposite of 
            /// the provided direction to prevent a corridor going back on it's self.
            /// 
            /// The parameter pDirExclude is the first direction chosen for a corridor, and
            /// to prevent it from being used will prevent a corridor from going back on 
            /// itself
            /// </summary>
            /// <param name="pDir">Current direction</param>
            /// <param name="pDirExclude">Direction to exclude</param>
            /// <returns></returns>
            private Point GetDirection(Point pDir, Point pDirExclude)
            {
                Point newDir;
                do
                {
                    newDir = DirectionsStraight[random.Next(0, DirectionsStraight.GetLength(0))];
                } while (ReverseDirection(newDir) == pDir || ReverseDirection(newDir) == pDirExclude);


                return newDir;
            }

            private Point ReverseDirection(Point pDir)
            {
                return new Point(-pDir.X, -pDir.Y);
            }

            #endregion

            #region room test

            /// <summary>
            /// Check if rctCurrentRoom can be built
            /// </summary>
            /// <returns>Bool indicating success</returns>
            private bool VerifyRoom()
            {
                //make it one bigger to ensure that testing gives it a border
                rctCurrentRoom.Inflate(RoomDistance, RoomDistance);

                //check it occupies legal, empty coordinates
                for (int x = rctCurrentRoom.Left; x <= rctCurrentRoom.Right; x++)
                    for (int y = rctCurrentRoom.Top; y <= rctCurrentRoom.Bottom; y++)
                        if (!ValidatePoint(x, y) || !IsFilledPoint(x, y))
                            return false;

                //check it doesn't encroach onto existing rooms
                foreach (Rectangle r in rctBuiltRooms)
                    if (r.IntersectsWith(rctCurrentRoom))
                        return false;

                rctCurrentRoom.Inflate(-RoomDistance, -RoomDistance);

                //check the room is the specified distance away from corridors
                rctCurrentRoom.Inflate(CorridorDistance, CorridorDistance);

                foreach (Point p in lBuiltCorridors)
                    if (rctCurrentRoom.Contains(p))
                        return false;

                rctCurrentRoom.Inflate(-CorridorDistance, -CorridorDistance);

                return true;
            }

            /// <summary>
            /// Add the global Room to the rooms collection and draw it on the map
            /// </summary>
            private void BuildRoom(bool placeStairs)
            {
                rctBuiltRooms.Add(rctCurrentRoom);

                for (int x = rctCurrentRoom.Left; x <= rctCurrentRoom.Right; x++)
                    for (int y = rctCurrentRoom.Top; y <= rctCurrentRoom.Bottom; y++)
                        Map[x, y] = EmptyCell;

                if (placeStairs)
                {
                    var centerX = (int)Math.Round((rctCurrentRoom.Left + rctCurrentRoom.Right) / 2d);
                    var centerY = (int)Math.Round((rctCurrentRoom.Top + rctCurrentRoom.Bottom) / 2d);

                    Map[centerX, centerY] = TrapDoorCell;
                }
            }

            #endregion

            #region Map Utilities

            /// <summary>
            /// Check if the Point falls within the map array range
            /// </summary>
            /// <param name="x">x to test</param>
            /// <param name="y">y to test</param>
            /// <returns>Is Point with map array?</returns>
            private bool ValidatePoint(int x, int y)
            {
                return x >= 0 & x < Map.GetLength(0) & y >= 0 & y < Map.GetLength(1);
            }

            /// <summary>
            /// Set array Point to specified value
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="val"></param>
            private void SetPoint(int x, int y, int val)
            {
                Map[x, y] = val;
            }

            /// <summary>
            /// Get the value of the specified Point
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            private int GetPoint(int x, int y)
            {
                return Map[x, y];
            }

            private bool IsFilled(int value)
            {
                return value == FilledCell;
            }

            private bool IsFilledPoint(int x, int y)
            {
                var point = GetPoint(x, y);
                return IsFilled(point);
            }

            #endregion
        }
    }
}