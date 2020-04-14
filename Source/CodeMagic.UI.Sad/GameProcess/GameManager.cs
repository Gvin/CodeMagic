using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Drawing;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class GameManager
    {
        public CurrentGame.GameCore<Player> StartGame()
        {
            var player = CreatePlayer();

            var startMap = new DungeonMapGenerator(Properties.Settings.Default.DebugWriteMapToFile).GenerateNewMap(1, out var playerPosition);
            CurrentGame.Initialize(startMap, player, playerPosition);
            var game = (CurrentGame.GameCore<Player>) CurrentGame.Game;
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

#if DEBUG
            player.Inventory.AddItem(CreateBanHammer());
#endif

            return player;
        }

        private IItem CreateBanHammer()
        {
            return new WeaponItem(new WeaponItemConfiguration
            {
                Name = "Ban Hammer",
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