using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.JournalMessages.Scenario;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.Saving;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        public static GameManager Current { get; } = new GameManager();

        private Task saveGameTask;
        private int turnsSinceLastSaving;

        private GameManager()
        {
            DungeonMapGenerator.Initialize(ImagesStorage.Current, Properties.Settings.Default.DebugWriteMapToFile);
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
            new SaveManager().SaveGame();

            turnsSinceLastSaving = 0;

            return game;
        }

        public void LoadGame()
        {
            var (game, data) = new SaveManager().LoadGame();

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

            if (turnsSinceLastSaving >= Properties.Settings.Default.SavingInterval)
            {
                saveGameTask?.Wait();
                saveGameTask = new SaveManager().SaveGameAsync();
                turnsSinceLastSaving = 0;
            }

            if (CurrentGame.Player.Health <= 0)
            {
                saveGameTask?.Wait();
                ((GameCore<Player>)CurrentGame.Game).TurnEnded -= game_TurnEnded;
                CurrentGame.Load(null);
                new SaveManager().DeleteSave();
            }
        }

        private Player CreatePlayer()
        {
            var player = new Player();

            var itemsGenerator = ItemsGeneratorManager.Generator;

            var weapon = new TorchItem();
            player.Inventory.AddItem(weapon);
            player.Equipment.EquipWeapon(weapon, true);

            var spellBook = itemsGenerator.GenerateSpellBook(ItemRareness.Trash);
            player.Inventory.AddItem(spellBook);
            player.Equipment.EquipItem(spellBook);

            player.Inventory.AddItem(GenerateStartingUsable());
            player.Inventory.AddItem(GenerateStartingUsable());

#if DEBUG
            player.Inventory.AddItem(CreateBanHammer());
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

        private IItem CreateBanHammer()
        {
            return new WeaponItem(new WeaponItemConfiguration
            {
                Name = "Ban Hammer",
                MaxDurability = 100000000,
                Description = new []
                {
                    "Powerful weapon designed to give his owner",
                    "the power of God. For testing purpose mostly."
                },
                HitChance = 100,
                Key = "weapon_ban_hammer",
                Weight = 0,
                LightPower = LightLevel.Medium,
                InventoryImage = ImagesStorage.Current.GetImage("Weapon_BanHammer"),
                WorldImage = ImagesStorage.Current.GetImage("ItemsOnGround_Weapon_Mace"),
                Rareness = ItemRareness.Epic,
                MinDamage = new Dictionary<Element, int>
                {
                    {Element.Acid, 100},
                    {Element.Blunt, 100},
                    {Element.Electricity, 100},
                    {Element.Fire, 100},
                    {Element.Frost, 100},
                    {Element.Piercing, 100},
                    {Element.Slashing, 100},
                    {Element.Magic, 100}
                },
                MaxDamage = new Dictionary<Element, int>
                {
                    {Element.Acid, 200},
                    {Element.Blunt, 200},
                    {Element.Electricity, 200},
                    {Element.Fire, 200},
                    {Element.Frost, 200},
                    {Element.Piercing, 200},
                    {Element.Slashing, 200},
                    {Element.Magic, 200}
                }
            });
        }
    }
}