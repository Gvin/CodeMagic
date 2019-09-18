using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Locations;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Implementations.Items;
using CodeMagic.Implementations.Objects.Creatures;
using CodeMagic.MapGeneration.GlobalWorld;
using CodeMagic.MapGeneration.Home;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        private const int GlobalWorldMapSize = 60;
        private const int HomeAreaMapSize = 60;

        public GameCore StartGame()
        {
            var player = CreatePlayer();
            var startingLocation = CreateHomeLocation();
            var game = new GameCore(startingLocation, player);
            startingLocation.CurrentArea.Refresh(game.GameTime);

            game.World.AddLocation(CreateGlobalWorldLocation());

            return game;
        }

        private ILocation CreateHomeLocation()
        {
            var map = new HomeLocationMapGenerator().GenerateMap(HomeAreaMapSize, HomeAreaMapSize, out var playerPosition);
            return new SimpleLocation(HomeLocationMapGenerator.LocationId, map, playerPosition);
        }

        private GlobalWorldLocation CreateGlobalWorldLocation()
        {
            var map = new GlobalWorldMapGenerator().GenerateMap(GlobalWorldMapSize, GlobalWorldMapSize, out var playerPos);
            return new GlobalWorldLocation(GlobalWorldMapGenerator.LocationId, map)
            {
                PlayerPosition = playerPos
            };
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
    }
}