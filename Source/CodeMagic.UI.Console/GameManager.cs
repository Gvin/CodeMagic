using System.IO;
using CodeMagic.Configuration.Xml;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures.Implementations;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.UI.Console
{
    public class GameManager
    {
        private const int GoblinsCount = 20;
        private const int MapSize = 31; // uneven value only!
        private const bool UseFakeMap = false;

        public GameCore StartGame()
        {
            var config = LoadConfiguration();
            ConfigurationManager.InitializeConfiguration(config);

            var map = CreateMap(UseFakeMap, out var playerPosition);

            var player = CreatePlayer();
            map.AddObject(playerPosition, player);

            return new GameCore(map, playerPosition);
        }

        private IConfigurationProvider LoadConfiguration()
        {
            using (var spellsConfig = File.OpenRead(@".\Configuration\Spells.xml"))
            using (var physicsConfig = File.OpenRead(@".\Configuration\Physics.xml"))
            using (var liquidsConfig = File.OpenRead(@".\Configuration\Liquids.xml"))
            {
                return ConfigurationProvider.Load(spellsConfig, physicsConfig, liquidsConfig);
            }
        }

        private IAreaMap CreateMap(bool fake, out Point playerPosition)
        {
            if (fake)
                return CreateFakeMap(out playerPosition);

            var realMap = new MapGenerator(FloorTypes.Stone).Generate(MapSize, MapSize, out playerPosition);
            PlaceTestGoblins(GoblinsCount, realMap, playerPosition);
            return realMap;
        }

        private IAreaMap CreateFakeMap(out Point playerPosition)
        {
            playerPosition = new Point(0, 0);

            var map = new AreaMap(10, 10);

            var cell1 = map.GetCell(1, 0);
            //cell1.Environment.Temperature = ;
            cell1.Objects.AddLiquid(new OilLiquidObject(100));

            var cell2 = map.GetCell(2, 0);
            //cell2.Environment.Temperature = -100;
            cell2.Objects.AddLiquid(new OilLiquidObject(100));

            var cell3 = map.GetCell(3, 0);
            cell3.Environment.Temperature = 300;
            cell3.Objects.AddLiquid(new OilLiquidObject(100));

            var cell4 = map.GetCell(4, 0);
            //cell4.Environment.Temperature = -100;
            cell4.Objects.AddLiquid(new OilLiquidObject(100));

            var cell5 = map.GetCell(5, 0);
            //cell5.Environment.Temperature = -100;
            cell5.Objects.AddLiquid(new OilLiquidObject(100));

            return map;
        }

        private void PlaceTestGoblins(int count, IAreaMap map, Point playerPosition)
        {
            var placed = 0;
            while (placed < count)
            {
                var x = RandomHelper.GetRandomValue(0, 30);
                var y = RandomHelper.GetRandomValue(0, 30);
                if (!map.ContainsCell(x, y))
                    continue;

                if (playerPosition.X == x && playerPosition.Y == y)
                    continue;

                var cell = map.GetCell(x, y);
                if (cell.BlocksMovement)
                    continue;

                var goblin = CreateGoblin();
                map.AddObject(new Point(x ,y), goblin);
                placed++;
            }
        }

        private WeaponItem CreateWoodenSword()
        {
            return new WeaponItem(new WeaponItemConfiguration
            {
                Name = "Wooden Sword",
                Key = "wooden_sword",
                DamageMin = 2,
                DamageMax = 5,
                Rareness = ItemRareness.Trash,
                Weight = 10
            });
        }

        private WeaponItem CreateElvesBlade()
        {
            return new WeaponItem(new WeaponItemConfiguration
            {
                Name = "Elves Blade",
                Key = "elves_blade",
                DamageMin = 10,
                DamageMax = 25,
                Rareness = ItemRareness.Rare,
                Weight = 7
            });
        }

        private IMapObject CreateGoblin()
        {
            return new GoblinCreatureObject(new GoblinCreatureObjectConfiguration
            {
                Name = "Goblin",
                Health = 20,
                MaxHealth = 20,
                MinDamage = 2,
                MaxDamage = 5,
                ViewDistance = 3
            });
        }

        private IPlayer CreatePlayer()
        {
            var player = new Player(new PlayerConfiguration
            {
                Health = 100,
                MaxHealth = 100,
                Mana = 1000,
                MaxMana = 1000,
                ManaRegeneration = 1,
                VisionRange = 4,
                MaxWeight = 100
            });

            var spellBook = new SpellBook(new SpellBookConfiguration
            {
                Name = "Book of Fire",
                Size = 10,
                Rareness = ItemRareness.Epic
            });

            player.Equipment.SpellBook = spellBook;

            var weapon = CreateWoodenSword();
            player.Equipment.Weapon = weapon;

            return player;
        }
    }
}