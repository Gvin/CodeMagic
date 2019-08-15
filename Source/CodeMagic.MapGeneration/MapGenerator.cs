using System.IO;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.MapGeneration.MapGenerators;

namespace CodeMagic.MapGeneration
{
    public class MapGenerator
    {
        public IAreaMap GenerateMap(
            MapSize size, 
            FloorTypes floorType,
            WallObjectConfiguration.WallType wallType, 
            out Point playerPosition, 
            bool writeMapFile = false)
        {
            var wallsFactory = new WallsFactory(wallType);
            var generator = CreateGenerator(wallsFactory);
            var map = generator.Generate(size, out playerPosition);

            ApplyFloorType(map, floorType);
            
            new ObjectsGenerator().GenerateObjects(map);

            new NpcGenerator().GenerateNpc(map, playerPosition);

            if (writeMapFile)
            {
                WriteMapToFile(map, playerPosition);
            }

            return map;
        }

        private void WriteMapToFile(IAreaMap map, Point playerPosition)
        {
            using (var file = File.CreateText(@".\Map.txt"))
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var line = string.Empty;
                    for (int x = 0; x < map.Width; x++)
                    {
                        var cell = map.GetCell(x, y);
                        if (cell.BlocksEnvironment)
                        {
                            line += "█";
                        }
                        else
                        {
                            if (playerPosition.X == x && playerPosition.Y == y)
                            {
                                line += "+";
                            }
                            else
                            {
                                line += " ";
                            }
                        }
                    }
                    file.WriteLine(line);
                }
            }
        }

        private void ApplyFloorType(IAreaMap map, FloorTypes floorType)
        {
            for (int y = 0; y < map.Height; y++)
            for (int x = 0; x < map.Width; x++)
            {
                map.GetCell(x, y).FloorType = floorType;
            }
        }

        private IMapGenerator CreateGenerator(WallsFactory wallsFactory)
        {
            return new DungeonMapGenerator(wallsFactory);
        }
    }

    public enum MapSize
    {
        Small,
        Medium,
        Big
    }
}