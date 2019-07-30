using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures.Implementations;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Statuses;
using CodeMagic.MapGeneration;
using CodeMagic.Objects.Implementation.Creatures;
using CodeMagic.Objects.Implementation.Creatures.NonPlayable;
using CodeMagic.Objects.Implementation.SolidObjects;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        private const int GoblinsCount = 20;
        private const int MapSize = 31; // uneven value only!
        private const bool UseFakeMap = false;
        private const LightLevel DefaultLightLevel = LightLevel.Darkness;

        public GameCore StartGame()
        {
            var map = CreateMap(UseFakeMap, out var playerPosition);

            var player = CreatePlayer();
            map.AddObject(playerPosition, player);

            map.Refresh();

            return new GameCore(map, playerPosition);
        }

        private IAreaMap CreateMap(bool fake, out Point playerPosition)
        {
            if (fake)
                return CreateFakeMap(out playerPosition);

            var realMap = new MapGenerator(FloorTypes.Stone).Generate(MapSize, MapSize, DefaultLightLevel, out playerPosition);
            PlaceTestGoblins(GoblinsCount, realMap, playerPosition);
            return realMap;
        }

        private IAreaMap CreateFakeMap(out Point playerPosition)
        {
            playerPosition = new Point(0, 0);

            var map = new AreaMap(10, 10, LightLevel.Dim2);

            map.AddObject(1, 3, Injector.Current.Create<IOilLiquidObject>(100));
            map.AddObject(2, 3, Injector.Current.Create<IOilLiquidObject>(100));
            map.AddObject(3, 3, Injector.Current.Create<IOilLiquidObject>(100));
            map.GetCell(3, 3).Environment.Temperature = 2400;

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
                map.AddObject(new Point(x, y), goblin);
                placed++;
            }
        }

        private WeaponItem CreateWoodenSword()
        {
            return new WeaponItem(new WeaponItemConfiguration
            {
                Name = "Wooden Sword",
                Key = "wooden_sword",
                MinDamage = 2,
                MaxDamage = 5,
                Rareness = ItemRareness.Trash,
                Weight = 10,
                HitChance = 70
            });
        }

        private WeaponItem CreateElvesBlade()
        {
            return new WeaponItem(new WeaponItemConfiguration
            {
                Name = "Elves Blade",
                Key = "elves_blade",
                MinDamage = 10,
                MaxDamage = 25,
                Rareness = ItemRareness.Rare,
                Weight = 7,
                HitChance = 90
            });
        }

        private IMapObject CreateGoblin()
        {
            return new GoblinImpl(new GoblinCreatureObjectConfiguration
            {
                Name = "Goblin",
                Health = 20,
                MaxHealth = 20,
                MinDamage = 2,
                MaxDamage = 5,
                HitChance = 60,
                VisibilityRange = 3
            });
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
            player.Equipment.EquipWeapon(weapon);

            return player;
        }
    }
}