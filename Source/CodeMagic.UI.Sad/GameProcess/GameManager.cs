using System.Linq;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Locations;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Implementations.Items;
using CodeMagic.Implementations.Items.Materials;
using CodeMagic.Implementations.Items.Usable;
using CodeMagic.Implementations.Objects.Creatures;
using CodeMagic.MapGeneration.GlobalWorld;
using CodeMagic.MapGeneration.Home;
using CodeMagic.UI.Sad.Views;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        private const int GlobalWorldMapSize = 60;
        private const int HomeAreaMapSize = 60;

        private TravelInProgressView travelInProgressView;

        public GameCore StartGame()
        {
            var player = CreatePlayer();
            var startingLocation = CreateHomeLocation(out var playerPosition);
            var game = new GameCore(startingLocation, player, playerPosition);
            startingLocation.CurrentArea.Refresh(game.GameTime);

            var globalWorldLocation = CreateGlobalWorldLocation();
            globalWorldLocation.CurrentArea.Refresh(game.GameTime);
            game.World.AddLocation(globalWorldLocation);

            game.World.TravelStarted += (sender, args) =>
            {
                travelInProgressView = new TravelInProgressView();
                travelInProgressView.Show();
            };

            game.World.TravelFinished += (sender, args) =>
            {
                travelInProgressView?.Close();
                travelInProgressView = null;
            };

            return game;
        }

        private ILocation CreateHomeLocation(out Point playerPosition)
        {
            var map = new HomeLocationMapGenerator().GenerateMap(HomeAreaMapSize, HomeAreaMapSize, out var enterPositions, out playerPosition);
            return new SimpleLocation(HomeLocationMapGenerator.LocationId, "Home", map, enterPositions, true);
        }

        private GlobalWorldLocation CreateGlobalWorldLocation()
        {
            var map = new GlobalWorldMapGenerator().GenerateMap(GlobalWorldMapSize, GlobalWorldMapSize, out var playerHomePosition);
            return new GlobalWorldLocation(GlobalWorldMapGenerator.LocationId, map, playerHomePosition);
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

            foreach (var building in ConfigurationManager.Current.Buildings.Buildings.Where(building => building.AutoUnlock))
            {
                player.UnlockBuilding(building);
            }

            var itemsGenerator = Injector.Current.Create<IItemsGenerator>();

            var weapon = new TorchItem();
            player.Inventory.AddItem(weapon);
            player.Equipment.EquipItem(weapon);

            var spellBook = itemsGenerator.GenerateSpellBook(ItemRareness.Trash);
            player.Inventory.AddItem(spellBook);
            player.Equipment.EquipItem(spellBook);

            player.Inventory.AddItem(itemsGenerator.GenerateUsable(ItemRareness.Common));
            player.Inventory.AddItem(itemsGenerator.GenerateUsable(ItemRareness.Common));

            player.Inventory.AddItem(new TeleporterStoneInactive());

            player.Inventory.AddItem(new Wood());
            player.Inventory.AddItem(new Wood());

            return player;
        }
    }
}