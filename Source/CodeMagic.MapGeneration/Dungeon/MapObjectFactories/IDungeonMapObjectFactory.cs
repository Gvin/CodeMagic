using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;

namespace CodeMagic.MapGeneration.Dungeon.MapObjectFactories
{
    public interface IDungeonMapObjectFactory
    {
        IMapObject CreateExitPortal();

        IMapObject CreateFloor();

        IMapObject CreateStairs();

        IMapObject CreateDoor();

        IMapObject CreateTrapDoor();

        IMapObject CreateWall();

        IMapObject CreateIndestructibleWall();

        IMapObject CreateTorchWall();

        IMapObject CreateOreWall(ItemRareness rareness);

        IMapObject CreateWall(int torchChance);
    }
}