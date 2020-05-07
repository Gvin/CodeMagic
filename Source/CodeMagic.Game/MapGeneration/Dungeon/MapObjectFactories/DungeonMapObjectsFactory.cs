using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.Floor;
using CodeMagic.Game.Objects.SolidObjects;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories
{
    internal class DungeonMapObjectsFactory : IMapObjectFactory
    {
        private const int SpikedWallChance = 10;

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
            return new DungeonTrapDoor();
        }

        public IMapObject CreateWall()
        {
            if (RandomHelper.CheckChance(SpikedWallChance))
                return new SpikedDungeonWall();

            return new DungeonWall();
        }

        public IMapObject CreateIndestructibleWall()
        {
            return CreateWall();
        }

        public IMapObject CreateTorchWall()
        {
            return new DungeonTorchWall();
        }

        public IMapObject CreateOreWall(ItemRareness rareness)
        {
            throw new InvalidOperationException("Ore walls are not supported for dungeon maps");
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