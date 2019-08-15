using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Implementations.Objects.Creatures;
using CodeMagic.Implementations.Objects.SolidObjects;
using CodeMagic.MapGeneration;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        private const MapSize MapSize = MapGeneration.MapSize.Small;
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

            return new MapGenerator().GenerateMap(
                MapSize,
                FloorTypes.Stone, 
                WallObjectConfiguration.WallType.Stone, 
                out playerPosition,
                true);
        }

        private IAreaMap CreateFakeMap(out Point playerPosition)
        {
            playerPosition = new Point(0, 0);

            var map = new AreaMap(10, 10);

            map.AddObject(3, 3, new TorchWallImpl(new TorchWallObjectConfiguration
            {
                LightPower = LightLevel.Bright1,
                Type = WallObjectConfiguration.WallType.Stone
            }));

            return map;
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
                MaxCarryWeight = 100
            });

            var itemsGenerator = Injector.Current.Create<IItemsGenerator>();

            var weapon = itemsGenerator.GenerateWeapon(ItemRareness.Trash);
            player.Inventory.AddItem(weapon);
            player.Equipment.EquipItem(weapon);

            var spellBook = itemsGenerator.GenerateSpellBook(ItemRareness.Trash);
            player.Inventory.AddItem(spellBook);
            player.Equipment.EquipItem(spellBook);

            return player;
        }
    }
}