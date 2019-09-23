using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Implementations.Objects.Floor;
using CodeMagic.Implementations.Objects.SolidObjects;

namespace CodeMagic.MapGeneration.Dungeon
{
    internal class DungeonMapObjectsFactory
    {
        private readonly WallObjectConfiguration.WallType wallType;

        public DungeonMapObjectsFactory(WallObjectConfiguration.WallType wallType)
        {
            this.wallType = wallType;
        }

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

        public DoorObject CreateDoor(bool horizontal)
        {
            return new DoorObjectImpl(horizontal);
        }

        public IMapObject CreateTrapDoor()
        {
            return new TrapDoorObjectImpl();
        }

        public WallObject CreateWall()
        {
            return new WallImpl(new WallObjectConfiguration
            {
                Name = "Wall",
                Type = wallType
            });
        }

        public WallObject CreateTorchWall()
        {
            return new TorchWallImpl(new TorchWallObjectConfiguration
            {
                Name = "Torch Wall",
                Type = wallType,
                LightPower = LightLevel.Medium
            });
        }

        public WallObject CreateWall(int torchChance)
        {
            if (RandomHelper.CheckChance(torchChance))
            {
                return CreateTorchWall();
            }

            return CreateWall();
        }
    }
}