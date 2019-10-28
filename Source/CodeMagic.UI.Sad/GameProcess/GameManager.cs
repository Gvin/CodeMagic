using System.Linq;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Game.Locations;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.MapGeneration.GlobalWorld;
using CodeMagic.Game.MapGeneration.Home;
using CodeMagic.UI.Sad.Views;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        private const int GlobalWorldMapSize = 60;
        private const int HomeAreaMapSize = 60;

        private TravelInProgressView travelInProgressView;

        public GameCore<Player> StartGame()
        {
            var player = CreatePlayer();
            var startingLocation = CreateHomeLocation(out var playerPosition);
            var game = new GameCore<Player>(startingLocation, player, playerPosition);
            startingLocation.CurrentArea.Refresh(game.GameTime);

            player.Inventory.ItemAdded += (sender, args) =>
            {
                game.Journal.Write(new ItemReceivedMessage(args.Item));
            };
            player.Inventory.ItemRemoved += (sender, args) =>
            {
                game.Journal.Write(new ItemLostMessage(args.Item));
            };

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

        private Player CreatePlayer()
        {
            var player = new Player();

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

            player.Inventory.AddItem(itemsGenerator.GenerateLumberjackAxe(ItemRareness.Trash));
            player.Inventory.AddItem(itemsGenerator.GeneratePickaxe(ItemRareness.Trash));

            player.Inventory.AddItem(new WateringCan());

            return player;
        }
    }
}