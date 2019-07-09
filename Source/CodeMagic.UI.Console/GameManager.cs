using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures.Implementations;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Objects.SolidObjects;

namespace CodeMagic.UI.Console
{
    public class GameManager
    {
        public GameCore StartGame()
        {
            var map = new MapGenerator(FloorTypes.Stone).Generate(31, 31, out var playerPosition);

            var player = CreatePlayer();
            map.AddObject(playerPosition, player);

            PlaceTestGoblins(20, map);

            return new GameCore(map, playerPosition);
        }

        private void PlaceTestGoblins(int count, IAreaMap map)
        {
            var placed = 0;
            while (placed < count)
            {
                var x = RandomHelper.GetRandomValue(0, 30);
                var y = RandomHelper.GetRandomValue(0, 30);
                if (!map.ContainsCell(x, y))
                    continue;

                var cell = map.GetCell(x, y);
                if (cell.BlocksMovement)
                    continue;

                var goblin = CreateGoblin();
                map.AddObject(new Point(x ,y), goblin);
                placed++;
            }
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

        private AreaMap CreateFakeMap(out Point playerPosition)
        {
            var map = new AreaMap(7, 7);
            map.GetCell(2, 2).Objects.Add(new SolidObject(new SolidObjectConfiguration
            {
                Type = SolidObjectConfiguration.ObjectTypeWallStone
            }));
            map.GetCell(2, 3).Objects.Add(new SolidObject(new SolidObjectConfiguration
            {
                Type = SolidObjectConfiguration.ObjectTypeWallStone
            }));
            map.GetCell(3, 3).Objects.Add(new SolidObject(new SolidObjectConfiguration
            {
                Type = SolidObjectConfiguration.ObjectTypeWallStone
            }));
            playerPosition = new Point(0, 0);
            return map;
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

            return player;
        }
    }
}