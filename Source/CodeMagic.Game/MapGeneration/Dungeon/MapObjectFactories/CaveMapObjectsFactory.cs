using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items.Materials;
using CodeMagic.Game.Objects.Floor;
using CodeMagic.Game.Objects.SolidObjects;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories
{
    public class CaveMapObjectsFactory : IDungeonMapObjectFactory
    {
        private static readonly Dictionary<ItemRareness, MetalType[]> OreTypes =
            new Dictionary<ItemRareness, MetalType[]>
            {
                {
                    ItemRareness.Common, new[]
                    {
                        MetalType.Copper,
                        MetalType.Iron
                    }
                },
                {
                    ItemRareness.Uncommon, new[]
                    {
                        MetalType.Silver,
                        MetalType.ElvesMetal,
                        MetalType.DwarfsMetal
                    }
                },
                {
                    ItemRareness.Rare, new[]
                    {
                        MetalType.Mythril,
                        MetalType.Adamant
                    }
                }
            };

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
            return new MinableCaveWall();
        }

        public IMapObject CreateIndestructibleWall()
        {
            return new CaveWall();
        }

        public IMapObject CreateTorchWall()
        {
            return new DungeonTorchWall();
        }

        public IMapObject CreateOreWall(ItemRareness rareness)
        {
            if (OreTypes.ContainsKey(rareness))
            {
                var metalType = RandomHelper.GetRandomElement(OreTypes[rareness]);
                return new OreCaveWall(metalType);
            }

            return CreateWall();
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