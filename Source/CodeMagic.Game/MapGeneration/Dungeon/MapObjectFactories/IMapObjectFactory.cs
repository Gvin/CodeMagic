using CodeMagic.Core.Objects;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories
{
    public interface IMapObjectFactory
    {
        IMapObject CreateFloor();

        IMapObject CreateStairs();

        IMapObject CreateDoor();

        IMapObject CreateTrapDoor();

        IMapObject CreateWall();

        IMapObject CreateTorchWall();

        IMapObject CreateWall(int torchChance);
    }
}