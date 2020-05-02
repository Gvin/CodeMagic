using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.Floor;
using CodeMagic.Game.Objects.Furniture;
using CodeMagic.Game.Objects.SolidObjects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.MapGeneration.Dungeon
{
    internal partial class DungeonObjectsGenerator
    {
        private static ObjectsPattern CreateTableWithChairs(IImagesStorage storage)
        {
            var pattern = new ObjectsPattern(5, 3, 20, 5);
            pattern.Add(1, 1, () => new FurnitureObject(new FurnitureObjectConfiguration
            {
                Name = "Chair",
                MaxHealth = 10,
                MaxWoodCount = 2,
                MinWoodCount = 0,
                Size = ObjectSize.Medium,
                ZIndex = ZIndex.BigDecoration,
                WorldImage = storage.GetImage("Furniture_Chair_Right")
            }));
            pattern.Add(2, 1, () => new FurnitureObject(new FurnitureObjectConfiguration
            {
                Name = "Table",
                MaxHealth = 20,
                MaxWoodCount = 4,
                MinWoodCount = 1,
                Size = ObjectSize.Big,
                ZIndex = ZIndex.BigDecoration,
                BlocksMovement = true,
                WorldImage = storage.GetImage("Furniture_Table")
            }));
            pattern.Add(3, 1, () => new FurnitureObject(new FurnitureObjectConfiguration
            {
                Name = "Chair",
                MaxHealth = 10,
                MaxWoodCount = 2,
                MinWoodCount = 0,
                Size = ObjectSize.Medium,
                ZIndex = ZIndex.BigDecoration,
                WorldImage = storage.GetImage("Furniture_Chair_Left")
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

        private static ObjectsPattern CreateShelfDown(IImagesStorage storage)
        {
            var pattern = new ObjectsPattern(3, 3, 40, 15);
            pattern.Add(1, 1, () => CreateShelf(storage.GetImage("Furniture_Shelf_Down")));
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

        private static ObjectsPattern CreateShelfUp(IImagesStorage storage)
        {
            var pattern = new ObjectsPattern(3, 3, 40, 15);
            pattern.Add(1, 1, () => CreateShelf(storage.GetImage("Furniture_Shelf_Up")));
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

        private static ObjectsPattern CreateShelfLeft(IImagesStorage storage)
        {
            var pattern = new ObjectsPattern(3, 3, 40, 15);
            pattern.Add(1, 1, () => CreateShelf(storage.GetImage("Furniture_Shelf_Left")));
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

        private static ObjectsPattern CreateShelfRight(IImagesStorage storage)
        {
            var pattern = new ObjectsPattern(3, 3, 40, 15);
            pattern.Add(1, 1, () => CreateShelf(storage.GetImage("Furniture_Shelf_Right")));
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

        private static ObjectsPattern CreateCrate(IImagesStorage storage)
        {
            var pattern = new ObjectsPattern(3, 3, 50, 10);

            pattern.Add(1, 1, () => new ContainerObject(new ContainerObjectConfiguration
            {
                Name = "Crate",
                BlocksMovement = true,
                MaxHealth = 20,
                Size = ObjectSize.Big,
                ZIndex = ZIndex.BigDecoration,
                MinWoodCount = 1,
                MaxWoodCount = 4,
                ContainerType = "crate",
                WorldImage = storage.GetImage("Furniture_Crate")
            }));

            pattern.AddRequirement(1, 1, RequirementIsEmpty);
            pattern.AddRequirement(1, 0, RequirementIsEmpty);
            pattern.AddRequirement(1, 2, RequirementIsEmpty);
            pattern.AddRequirement(0, 1, RequirementIsEmpty);
            pattern.AddRequirement(2, 1, RequirementIsEmpty);

            return pattern;
        }

        private static ObjectsPattern CreateChest(IImagesStorage storage)
        {
            var pattern = new ObjectsPattern(3, 3, 50, 5);

            pattern.Add(1, 1, () => new ContainerObject(new ContainerObjectConfiguration
            {
                Name = "Chest",
                BlocksMovement = true,
                MaxHealth = 30,
                Size = ObjectSize.Big,
                ZIndex = ZIndex.BigDecoration,
                MinWoodCount = 1,
                MaxWoodCount = 4,
                ContainerType = "chest",
                WorldImage = storage.GetImage("Furniture_Chest")
            }));

            pattern.AddRequirement(1, 1, RequirementIsEmpty);
            pattern.AddRequirement(1, 0, RequirementIsEmpty);
            pattern.AddRequirement(1, 2, RequirementIsEmpty);
            pattern.AddRequirement(0, 1, RequirementIsEmpty);
            pattern.AddRequirement(2, 1, RequirementIsEmpty);

            return pattern;
        }

        private static ObjectsPattern CreateGoldenChest(IImagesStorage storage)
        {
            var pattern = new ObjectsPattern(3, 3, 100, 1);

            pattern.Add(1, 1, () => new ContainerObject(new ContainerObjectConfiguration
            {
                Name = "Chest",
                BlocksMovement = true,
                MaxHealth = 30,
                Size = ObjectSize.Big,
                ZIndex = ZIndex.BigDecoration,
                MinWoodCount = 1,
                MaxWoodCount = 4,
                ContainerType = "chest",
                LootLevelIncrement = 1,
                WorldImage = storage.GetImage("Furniture_Chest_Golden")
            }));

            pattern.AddRequirement(1, 1, RequirementIsEmpty);
            pattern.AddRequirement(1, 0, RequirementIsEmpty);
            pattern.AddRequirement(1, 2, RequirementIsEmpty);
            pattern.AddRequirement(0, 1, RequirementIsEmpty);
            pattern.AddRequirement(2, 1, RequirementIsEmpty);

            return pattern;
        }

        private static bool RequirementIsWall(IAreaMapCell cell)
        {
            return cell.BlocksEnvironment && !cell.Objects.OfType<DoorBase>().Any();
        }

        private static bool RequirementIsAny(IAreaMapCell cell)
        {
            return true;
        }

        private static bool RequirementIsEmpty(IAreaMapCell cell)
        {
            return cell.Objects.All(obj => obj is FloorObject);
        }

        private static IMapObject CreateShelf(SymbolsImage image)
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
            });
        }

        private class ObjectsPattern
        {
            private readonly MapObjectsCollection[][] pattern;

            public ObjectsPattern(int width, int height, int rareness, int maxCount)
            {
                Width = width;
                Height = height;
                Rareness = rareness;
                MaxCount = maxCount;

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

            public int Rareness { get; }

            public int MaxCount { get; }

            public int Width { get; }

            public int Height { get; }

            public void Add(int x, int y, Func<IMapObject> objectFactory)
            {
                pattern[y][x].Add(objectFactory);
            }

            public void AddRequirement(int x, int y, Func<IAreaMapCell, bool> requirement)
            {
                pattern[y][x].AddRequirement(requirement);
            }

            public Func<IMapObject>[] Get(int x, int y)
            {
                return pattern[y][x].ToArray();
            }

            public bool CheckRequirements(int x, int y, IAreaMapCell cell)
            {
                return pattern[y][x].CheckRequirements(cell);
            }

            internal class MapObjectsCollection : List<Func<IMapObject>>
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