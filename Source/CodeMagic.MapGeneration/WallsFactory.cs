using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Implementations.Objects.SolidObjects;

namespace CodeMagic.MapGeneration
{
    internal class WallsFactory
    {
        private readonly WallObjectConfiguration.WallType wallType;

        public WallsFactory(WallObjectConfiguration.WallType wallType)
        {
            this.wallType = wallType;
        }

        public DoorObject CreateDoor()
        {
            return new DoorObjectImpl();
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