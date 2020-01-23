using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.Floor;
using CodeMagic.Game.Objects.SolidObjects;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories
{
    public class CaveMapObjectsFactory : IDungeonMapObjectFactory
    {
        private readonly IDungeonMapGenerator dungeonMapGenerator;

        public CaveMapObjectsFactory(IDungeonMapGenerator dungeonMapGenerator)
        {
            this.dungeonMapGenerator = dungeonMapGenerator;
        }

        public IMapObject CreateFloor()
        {
            return new FloorObject(FloorObject.Type.Stone);
        }

        public IMapObject CreateStairs()
        {
            return new DungeonStairs();
        }

        public IMapObject CreateDoor()
        {
            return new DungeonDoor();
        }

        public IMapObject CreateTrapDoor()
        {
            return new DungeonTrapDoor(dungeonMapGenerator);
        }

        public IMapObject CreateWall()
        {
            return new CaveWall();
        }

        public IMapObject CreateTorchWall()
        {
            return new DungeonTorchWall();
        }

        public IMapObject CreateWall(int torchChance)
        {
            if (RandomHelper.CheckChance(torchChance))
            {
                return CreateTorchWall();
            }

            return CreateWall();
        }
    }
}