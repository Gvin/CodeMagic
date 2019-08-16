using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Implementations.Objects.DecorativeObjects;
using CodeMagic.Implementations.Objects.SolidObjects;

namespace CodeMagic.MapGeneration
{
    internal class MapObjectsFactory
    {
        private readonly WallObjectConfiguration.WallType wallType;

        public MapObjectsFactory(WallObjectConfiguration.WallType wallType)
        {
            this.wallType = wallType;
        }

        public DecorativeObject CreateTrapDoor()
        {
            return new DecorativeObjectImpl(new DecorativeObjectConfiguration
            {
                Name = "Trap Door",
                BlocksMovement = false,
                Size = ObjectSize.Huge,
                Type = DecorativeObjectConfiguration.ObjectType.TrapDoor,
                ZIndex = ZIndex.AreaDecoration
            });
        }

        public DoorObject CreateDoor(bool horizontal)
        {
            return new DoorObjectImpl(horizontal);
        }

        public StairsObject CreateStairsUp()
        {
            return new StairsObjectImpl();
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