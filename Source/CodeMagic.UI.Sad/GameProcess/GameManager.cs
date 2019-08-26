using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Implementations.Items;
using CodeMagic.Implementations.Objects.Creatures;
using CodeMagic.Implementations.Objects.SolidObjects;
using CodeMagic.MapGeneration;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        private const bool UseFakeMap = false;

        public GameCore StartGame()
        {
            var player = CreatePlayer();
            return new GameCore(CreateMapGenerator(), player);
        }

        private IMapGenerator CreateMapGenerator()
        {
            if (UseFakeMap)
            {
                return new FakeMapGenerator();
            }

            return new MapGenerator(Properties.Settings.Default.DebugWriteMapToFile);
        }

        private IPlayer CreatePlayer()
        {
            var player = new PlayerImpl(new PlayerConfiguration
            {
                Health = 100,
                MaxHealth = 100,
                Mana = 1000,
                MaxMana = 1000,
                ManaRegeneration = 10,
                VisibilityRange = 4,
                MaxCarryWeight = 25000
            });

            var itemsGenerator = Injector.Current.Create<IItemsGenerator>();

            var weapon = new TorchItem();
            player.Inventory.AddItem(weapon);
            player.Equipment.EquipItem(weapon);

            var spellBook = itemsGenerator.GenerateSpellBook(ItemRareness.Trash);
            player.Inventory.AddItem(spellBook);
            player.Equipment.EquipItem(spellBook);

            player.Inventory.AddItem(itemsGenerator.GenerateUsable(ItemRareness.Common));
            player.Inventory.AddItem(itemsGenerator.GenerateUsable(ItemRareness.Common));

            return player;
        }

        private class FakeMapGenerator : IMapGenerator
        {
            public IAreaMap GenerateNewMap(int level, out Point playerPosition)
            {
                playerPosition = new Point(0, 0);

                var map = new AreaMap(10, 10);

                map.AddObject(3, 3, new TorchWallImpl(new TorchWallObjectConfiguration
                {
                    LightPower = LightLevel.Bright1,
                    Type = WallObjectConfiguration.WallType.Stone
                }));

                var monster = new MonstersGenerator().GenerateRandomMonster(1);
                map.AddObject(2, 2, monster);

                return map;
            }
        }
    }
}