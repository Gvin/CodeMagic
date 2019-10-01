using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Implementations.Objects.Floor;
using CodeMagic.Implementations.Objects.SolidObjects;

namespace CodeMagic.MapGeneration.Dungeon.MapObjectFactories
{
    public class CaveMapObjectsFactory : IDungeonMapObjectFactory
    {
        public IMapObject CreateExitPortal()
        {
            return new ExitPortalObject();
        }

        public IMapObject CreateFloor()
        {
            return new FloorObject(FloorObject.Type.Stone);
        }

        public IMapObject CreateStairs()
        {
            return new StairsObjectImpl();
        }

        public IMapObject CreateDoor()
        {
            return new DungeonDoor();
        }

        public IMapObject CreateTrapDoor()
        {
            return new TrapDoorObjectImpl();
        }

        public IMapObject CreateWall()
        {
            return new CaveWall(true);
        }

        public IMapObject CreateIndestructibleWall()
        {
            return new CaveWall(false);
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