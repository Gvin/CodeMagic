using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.Creatures.Implementations;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Implementations.Items.Usable;
using CodeMagic.Implementations.Objects.Creatures;
using CodeMagic.Implementations.Objects.Creatures.NonPlayable;
using CodeMagic.Implementations.Objects.SolidObjects;
using CodeMagic.MapGeneration;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        private const int GoblinsCount = 20;
        private const int MapSize = 31; // uneven value only!
        private const bool UseFakeMap = false;

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

            var realMap = new MapGenerator(FloorTypes.Stone).Generate(MapSize, MapSize, out playerPosition);
            PlaceTestGoblins(GoblinsCount, realMap, playerPosition);
            return realMap;
        }

        private IAreaMap CreateFakeMap(out Point playerPosition)
        {
            playerPosition = new Point(0, 0);

            var map = new AreaMap(10, 10);

            map.AddObject(3, 3, new TorchWallImpl(new TorchWallObjectConfiguration
            {
                LightPower = LightLevel.Bright1,
                Type = WallObjectConfiguration.ObjectTypeWallStone
            }));


            return map;
        }

        private void PlaceTestGoblins(int count, IAreaMap map, Point playerPosition)
        {
            var placed = 0;
            while (placed < count)
            {
                var x = RandomHelper.GetRandomValue(0, 30);
                var y = RandomHelper.GetRandomValue(0, 30);

                if (playerPosition.X == x && playerPosition.Y == y)
                    continue;

                var cell = map.TryGetCell(x, y);
                if (cell == null)
                    continue;

                if (cell.BlocksMovement)
                    continue;

                var goblin = CreateGoblin();
                map.AddObject(new Point(x, y), goblin);
                placed++;
            }
        }

        private NonPlayableCreatureObject CreateGoblin()
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

            player.Inventory.AddItem(new HealthRestorationItem(new HealthPotionItemConfiguration
            {
                Description = "Medium sized jar with bloody-red liquid.",
                HealValue = 50,
                ImageName = "Item_Health_Potion",
                Key = "health_potion",
                Name = "Health Potion",
                Rareness = ItemRareness.Uncommon,
                Weight = 1
            }));
            player.Inventory.AddItem(new ManaRestorationItem(new ManaRestorationItemConfiguration
            {
                Description = "Medium sized jar with bright blue liquid.",
                ManaRestoreValue = 50,
                ImageName = "Item_Mana_Potion",
                Key = "mana_potion",
                Name = "Mana Potion",
                Rareness = ItemRareness.Uncommon,
                Weight = 1
            }));

            player.Inventory.AddItem(new HealthRestorationItem(new HealthPotionItemConfiguration
            {
                Description = "A small phial with bloody-red liquid.",
                HealValue = 25,
                ImageName = "Item_Health_Potion_Small",
                Key = "health_potion_small",
                Name = "Small Health Potion",
                Rareness = ItemRareness.Common,
                Weight = 1
            }), 2);

            player.Inventory.AddItem(new ManaRestorationItem(new ManaRestorationItemConfiguration
            {
                Description = "A small phial with bright blue liquid.",
                ManaRestoreValue = 25,
                ImageName = "Item_Mana_Potion_Small",
                Key = "mana_potion_small",
                Name = "Small Mana Potion",
                Rareness = ItemRareness.Common,
                Weight = 1
            }), 2);


            var itemsGenerator = Injector.Current.Create<IItemsGenerator>();

            var weapon = itemsGenerator.GenerateWeapon(ItemRareness.Trash);
            player.Inventory.AddItem(weapon);

            for (int i = 0; i < 10; i++)
            {
                var rareness = (ItemRareness) RandomHelper.GetRandomValue(1, 3);
                player.Inventory.AddItem(itemsGenerator.GenerateSpellBook(rareness));
            }

            return player;
        }
    }
}