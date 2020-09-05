using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items.Materials;
using CodeMagic.Game.Objects.Floor;
using CodeMagic.Game.Objects.Furniture;
using CodeMagic.Game.Objects.LiquidObjects;
using CodeMagic.Game.Objects.SolidObjects;

namespace CodeMagic.Game.MapGeneration.Dungeon.ObjectsGenerators
{
    internal partial class DungeonObjectsGenerator
    {
        private static ObjectsPattern CreateTableWithChairs()
        {
            var pattern = new ObjectsPattern(5, 3, 0.0007);
            pattern.Add(1, 1, (level) => new FurnitureObject(new FurnitureObjectConfiguration
            {
                Name = "Chair",
                MaxHealth = 10,
                MaxWoodCount = 2,
                MinWoodCount = 0,
                Size = ObjectSize.Medium,
                ZIndex = ZIndex.BigDecoration,
                WorldImage = "Furniture_Chair_Right"
            }));
            pattern.Add(2, 1, (level) => new FurnitureObject(new FurnitureObjectConfiguration
            {
                Name = "Table",
                MaxHealth = 20,
                MaxWoodCount = 4,
                MinWoodCount = 1,
                Size = ObjectSize.Big,
                ZIndex = ZIndex.BigDecoration,
                BlocksMovement = true,
                WorldImage = "Furniture_Table"
            }));
            pattern.Add(3, 1, (level) => new FurnitureObject(new FurnitureObjectConfiguration
            {
                Name = "Chair",
                MaxHealth = 10,
                MaxWoodCount = 2,
                MinWoodCount = 0,
                Size = ObjectSize.Medium,
                ZIndex = ZIndex.BigDecoration,
                WorldImage = "Furniture_Chair_Left"
            }));
            
            pattern.AddRequirement(0, 0, RequirementIsEmpty);
            pattern.AddRequirement(1, 0, RequirementIsEmpty);
            pattern.AddRequirement(2, 0, RequirementIsEmpty);
            pattern.AddRequirement(3, 0, RequirementIsEmpty);
            pattern.AddRequirement(4, 0, RequirementIsEmpty);

            pattern.AddRequirement(0, 2, RequirementIsEmpty);
            pattern.AddRequirement(1, 2, RequirementIsEmpty);
            pattern.AddRequirement(2, 2, RequirementIsEmpty);
            pattern.AddRequirement(3, 2, RequirementIsEmpty);
            pattern.AddRequirement(4, 2, RequirementIsEmpty);

            return pattern;
        }

        private static ObjectsPattern CreateShelfDown()
        {
            var pattern = new ObjectsPattern(3, 3, 0.003);
            pattern.Add(1, 1, (level) => CreateShelf(level, "Furniture_Shelf_Down"));
            pattern.AddRequirement(1, 1, RequirementIsEmpty);
            pattern.AddRequirement(0, 2, RequirementIsEmpty);
            pattern.AddRequirement(1, 2, RequirementIsEmpty);
            pattern.AddRequirement(2, 2, RequirementIsEmpty);

            pattern.AddRequirement(0, 0, RequirementIsWall);
            pattern.AddRequirement(1, 0, RequirementIsWall);
            pattern.AddRequirement(2, 0, RequirementIsWall);

            pattern.AddRequirement(0, 1, RequirementIsAny);
            pattern.AddRequirement(2, 1, RequirementIsAny);

            return pattern;
        }

        private static ObjectsPattern CreateShelfUp()
        {
            var pattern = new ObjectsPattern(3, 3, 0.003);
            pattern.Add(1, 1, (level) => CreateShelf(level, "Furniture_Shelf_Up"));
            pattern.AddRequirement(1, 1, RequirementIsEmpty);
            pattern.AddRequirement(0, 0, RequirementIsEmpty);
            pattern.AddRequirement(1, 0, RequirementIsEmpty);
            pattern.AddRequirement(2, 0, RequirementIsEmpty);

            pattern.AddRequirement(0, 2, RequirementIsWall);
            pattern.AddRequirement(1, 2, RequirementIsWall);
            pattern.AddRequirement(2, 2, RequirementIsWall);

            pattern.AddRequirement(0, 1, RequirementIsAny);
            pattern.AddRequirement(2, 1, RequirementIsAny);

            return pattern;
        }

        private static ObjectsPattern CreateShelfLeft()
        {
            var pattern = new ObjectsPattern(3, 3, 0.003);
            pattern.Add(1, 1, (level) => CreateShelf(level, "Furniture_Shelf_Left"));
            pattern.AddRequirement(1, 1, RequirementIsEmpty);
            pattern.AddRequirement(0, 0, RequirementIsEmpty);
            pattern.AddRequirement(0, 1, RequirementIsEmpty);
            pattern.AddRequirement(0, 2, RequirementIsEmpty);

            pattern.AddRequirement(2, 0, RequirementIsWall);
            pattern.AddRequirement(2, 1, RequirementIsWall);
            pattern.AddRequirement(2, 2, RequirementIsWall);

            pattern.AddRequirement(1, 0, RequirementIsAny);
            pattern.AddRequirement(1, 2, RequirementIsAny);

            return pattern;
        }

        private static ObjectsPattern CreateShelfRight()
        {
            var pattern = new ObjectsPattern(3, 3, 0.003);
            pattern.Add(1, 1, (level) => CreateShelf(level, "Furniture_Shelf_Right"));
            pattern.AddRequirement(1, 1, RequirementIsEmpty);

            pattern.AddRequirement(2, 0, RequirementIsEmpty);
            pattern.AddRequirement(2, 1, RequirementIsEmpty);
            pattern.AddRequirement(2, 2, RequirementIsEmpty);

            pattern.AddRequirement(0, 0, RequirementIsWall);
            pattern.AddRequirement(0, 1, RequirementIsWall);
            pattern.AddRequirement(0, 2, RequirementIsWall);

            pattern.AddRequirement(1, 0, RequirementIsAny);
            pattern.AddRequirement(1, 2, RequirementIsAny);

            return pattern;
        }

        private static ObjectsPattern CreateCrate()
        {
            var pattern = new ObjectsPattern(3, 3, 0.003);

            pattern.Add(1, 1, (level) => new ContainerObject(new ContainerObjectConfiguration
            {
                Name = "Crate",
                BlocksMovement = true,
                MaxHealth = 20,
                Size = ObjectSize.Big,
                ZIndex = ZIndex.BigDecoration,
                MinWoodCount = 1,
                MaxWoodCount = 4,
                ContainerType = "crate",
                WorldImage = "Furniture_Crate"
            }, level));

            pattern.AddRequirement(1, 1, RequirementIsEmpty);

            pattern.AddRequirement(0, 0, RequirementNotBlocking);
            pattern.AddRequirement(1, 0, RequirementNotBlocking);
            pattern.AddRequirement(2, 0, RequirementNotBlocking);

            pattern.AddRequirement(0, 1, RequirementNotBlocking);
            pattern.AddRequirement(2, 1, RequirementNotBlocking);

            pattern.AddRequirement(0, 2, RequirementNotBlocking);
            pattern.AddRequirement(1, 2, RequirementNotBlocking);
            pattern.AddRequirement(2, 2, RequirementNotBlocking);

            return pattern;
        }

        private static ObjectsPattern CreateChest()
        {
            var pattern = new ObjectsPattern(3, 3, 0.0025);

            pattern.Add(1, 1, (level) => new ContainerObject(new ContainerObjectConfiguration
            {
                Name = "Chest",
                BlocksMovement = true,
                MaxHealth = 30,
                Size = ObjectSize.Big,
                ZIndex = ZIndex.BigDecoration,
                MinWoodCount = 1,
                MaxWoodCount = 4,
                ContainerType = "chest",
                WorldImage = "Furniture_Chest"
            }, level));

            pattern.AddRequirement(1, 1, RequirementIsEmpty);

            pattern.AddRequirement(0, 0, RequirementNotBlocking);
            pattern.AddRequirement(1, 0, RequirementNotBlocking);
            pattern.AddRequirement(2, 0, RequirementNotBlocking);

            pattern.AddRequirement(0, 1, RequirementNotBlocking);
            pattern.AddRequirement(2, 1, RequirementNotBlocking);

            pattern.AddRequirement(0, 2, RequirementNotBlocking);
            pattern.AddRequirement(1, 2, RequirementNotBlocking);
            pattern.AddRequirement(2, 2, RequirementNotBlocking);

            return pattern;
        }

        private static ObjectsPattern CreateGoldenChest()
        {
            var pattern = new ObjectsPattern(3, 3, 0.001);

            pattern.Add(1, 1, (level) => new ContainerObject(new ContainerObjectConfiguration
            {
                Name = "Chest",
                BlocksMovement = true,
                MaxHealth = 30,
                Size = ObjectSize.Big,
                ZIndex = ZIndex.BigDecoration,
                MinWoodCount = 1,
                MaxWoodCount = 4,
                ContainerType = "chest",
                LootLevelIncrement = 2,
                WorldImage = "Furniture_Chest_Golden"
            }, level));

            pattern.AddRequirement(1, 1, RequirementIsEmpty);

            pattern.AddRequirement(0, 0, RequirementNotBlocking);
            pattern.AddRequirement(1, 0, RequirementNotBlocking);
            pattern.AddRequirement(2, 0, RequirementNotBlocking);

            pattern.AddRequirement(0, 1, RequirementNotBlocking);
            pattern.AddRequirement(2, 1, RequirementNotBlocking);

            pattern.AddRequirement(0, 2, RequirementNotBlocking);
            pattern.AddRequirement(1, 2, RequirementNotBlocking);
            pattern.AddRequirement(2, 2, RequirementNotBlocking);

            return pattern;
        }

        private static ObjectsPattern CreateWaterPool()
        {
            var pattern = new ObjectsPattern(1, 1, 0.02);
            var volume = RandomHelper.GetRandomValue(20, 100);
            pattern.Add(0, 0, level => new WaterLiquid(volume));
            return pattern;
        }

        private static ObjectsPattern CreateSpikedFloor()
        {
            var pattern = new ObjectsPattern(3, 3, 0.01);

            pattern.Add(1, 1, level => new SpikedFloorObject());
            pattern.AddRequirement(1, 1, RequirementIsEmpty);

            pattern.AddRequirement(0, 0, RequirementNotBlocking);
            pattern.AddRequirement(1, 0, RequirementNotBlocking);
            pattern.AddRequirement(2, 0, RequirementNotBlocking);

            pattern.AddRequirement(0, 1, RequirementNotBlocking);
            pattern.AddRequirement(2, 1, RequirementNotBlocking);

            pattern.AddRequirement(0, 2, RequirementNotBlocking);
            pattern.AddRequirement(1, 2, RequirementNotBlocking);
            pattern.AddRequirement(2, 2, RequirementNotBlocking);

            return pattern;
        }

        private static ObjectsPattern CreateStone()
        {
            var pattern = new ObjectsPattern(1, 1, 0.03);
            pattern.Add(0, 0, level => new Stone());
            pattern.AddRequirement(0, 0, RequirementIsEmpty);
            return pattern;
        }

        private static bool RequirementIsWall(IAreaMapCell cell)
        {
            return cell.BlocksEnvironment && !cell.Objects.OfType<DoorBase>().Any();
        }

        private static bool RequirementNotBlocking(IAreaMapCell cell)
        {
            return !cell.BlocksMovement;
        }

        private static bool RequirementIsAny(IAreaMapCell cell)
        {
            return true;
        }

        private static bool RequirementIsEmpty(IAreaMapCell cell)
        {
            return cell.Objects.All(obj => obj is FloorObject);
        }

        private static IMapObject CreateShelf(int level, string image)
        {
            return new ContainerObject(new ContainerObjectConfiguration
            {
                Name = "Shelf",
                MaxHealth = 20,
                Size = ObjectSize.Big,
                ZIndex = ZIndex.BigDecoration,
                BlocksMovement = false,
                MinWoodCount = 0,
                MaxWoodCount = 1,
                ContainerType = "shelf",
                WorldImage = image
            }, level);
        }

        private class ObjectsPattern
        {
            private readonly MapObjectsCollection[][] pattern;

            public ObjectsPattern(int width, int height, double maxCountMultiplier)
            {
                Width = width;
                Height = height;
                MaxCountMultiplier = maxCountMultiplier;

                pattern = new MapObjectsCollection[height][];
                for (int y = 0; y < height; y++)
                {

                    pattern[y] = new MapObjectsCollection[width];
                    var row = pattern[y];
                    for (int x = 0; x < width; x++)
                    {
                        row[x] = new MapObjectsCollection();
                    }
                }
            }

            public double MaxCountMultiplier { get; }

            public int Width { get; }

            public int Height { get; }

            public void Add(int x, int y, Func<int, IMapObject> objectFactory)
            {
                pattern[y][x].Add(objectFactory);
            }

            public void AddRequirement(int x, int y, Func<IAreaMapCell, bool> requirement)
            {
                pattern[y][x].AddRequirement(requirement);
            }

            public Func<int, IMapObject>[] Get(int x, int y)
            {
                return pattern[y][x].ToArray();
            }

            public bool CheckRequirements(int x, int y, IAreaMapCell cell)
            {
                return pattern[y][x].CheckRequirements(cell);
            }

            internal class MapObjectsCollection : List<Func<int, IMapObject>>
            {
                private readonly List<Func<IAreaMapCell, bool>> requirements;

                public MapObjectsCollection()
                {
                    requirements = new List<Func<IAreaMapCell, bool>>();
                }

                public void AddRequirement(Func<IAreaMapCell, bool> requirement)
                {
                    requirements.Add(requirement);
                }

                public bool CheckRequirements(IAreaMapCell cell)
                {
                    if (requirements.Count == 0)
                    {
                        return true;
                    }
                    return requirements.All(requirement => requirement(cell));
                }
            }
        }
    }
}