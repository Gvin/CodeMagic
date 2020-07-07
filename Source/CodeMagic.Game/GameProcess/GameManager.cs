using System;
using System.Threading.Tasks;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Custom;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.JournalMessages.Scenario;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.GameProcess
{
    public interface IGameManager
    {
        GameCore<Player> StartGame();

        void LoadGame();
    }

    public class GameManager : IGameManager
    {
        private Task saveGameTask;
        private int turnsSinceLastSaving;
        private readonly ISaveService saveService;
        private readonly int savingInterval;

        public GameManager(ISaveService saveService, int savingInterval)
        {
            this.saveService = saveService;
            this.savingInterval = savingInterval;
        }

        public GameCore<Player> StartGame()
        {
            if (CurrentGame.Game is GameCore<Player> oldGame)
            {
                oldGame.TurnEnded -= game_TurnEnded;
            }

            GameData.Initialize(new GameData());

            var player = CreatePlayer();

            var startMap = DungeonMapGenerator.Current.GenerateNewMap(1, out var playerPosition);
            CurrentGame.Initialize(startMap, player, playerPosition);
            var game = (GameCore<Player>) CurrentGame.Game;
            startMap.Refresh();

            player.Inventory.ItemAdded += (sender, args) =>
            {
                game.Journal.Write(new ItemReceivedMessage(args.Item));
            };
            player.Inventory.ItemRemoved += (sender, args) =>
            {
                game.Journal.Write(new ItemLostMessage(args.Item));
            };

            game.Journal.Write(new StartGameMessage());

            game.TurnEnded += game_TurnEnded;
            saveService.SaveGame();

            turnsSinceLastSaving = 0;

            return game;
        }

        public void LoadGame()
        {
            var (game, data) = saveService.LoadGame();

            GameData.Initialize(data);
            CurrentGame.Load(game);

            if (game == null)
                return;

            game.Player.Inventory.ItemAdded += (sender, args) =>
            {
                game.Journal.Write(new ItemReceivedMessage(args.Item));
            };
            game.Player.Inventory.ItemRemoved += (sender, args) =>
            {
                game.Journal.Write(new ItemLostMessage(args.Item));
            };

            game.TurnEnded += game_TurnEnded;

            turnsSinceLastSaving = 0;
        }

        private void game_TurnEnded(object sender, EventArgs args)
        {
            turnsSinceLastSaving++;

            if (turnsSinceLastSaving >= savingInterval)
            {
                saveGameTask?.Wait();
                saveGameTask = saveService.SaveGameAsync();
                turnsSinceLastSaving = 0;
            }

            if (CurrentGame.Player.Health <= 0)
            {
                saveGameTask?.Wait();
                ((GameCore<Player>)CurrentGame.Game).TurnEnded -= game_TurnEnded;
                CurrentGame.Load(null);
                saveService.DeleteSave();
            }
        }

        private Player CreatePlayer()
        {
            var player = new Player();

            var itemsGenerator = ItemsGeneratorManager.Generator;

            var weapon = new TorchItem();
            player.Inventory.AddItem(weapon);
            player.Equipment.EquipHoldable(weapon, true);

            var spellBook = itemsGenerator.GenerateSpellBook(ItemRareness.Trash);
            player.Inventory.AddItem(spellBook);
            player.Equipment.EquipItem(spellBook);

            player.Inventory.AddItem(GenerateStartingUsable());
            player.Inventory.AddItem(GenerateStartingUsable());

#if DEBUG
            player.Inventory.AddItem(new BanHammer());
#endif

            return player;
        }

        private IItem GenerateStartingUsable()
        {
            while (true)
            {
                var usable = ItemsGeneratorManager.Generator.GenerateUsable(ItemRareness.Common);
                if (usable != null)
                    return usable;
            }

        }
    }
}