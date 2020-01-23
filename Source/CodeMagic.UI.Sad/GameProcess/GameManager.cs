using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        public GameCore<Player> StartGame()
        {
            var player = CreatePlayer();

            var startMap = new DungeonMapGenerator(Properties.Settings.Default.DebugWriteMapToFile).GenerateNewMap(1, out var playerPosition);
            var game = new GameCore<Player>(startMap, player, playerPosition);
            startMap.Refresh();

            player.Inventory.ItemAdded += (sender, args) =>
            {
                game.Journal.Write(new ItemReceivedMessage(args.Item));
            };
            player.Inventory.ItemRemoved += (sender, args) =>
            {
                game.Journal.Write(new ItemLostMessage(args.Item));
            };

            return game;
        }

        private Player CreatePlayer()
        {
            var player = new Player();

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