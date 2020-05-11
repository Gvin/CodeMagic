using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Objects.Creatures.Loot;

namespace CodeMagic.Game.Objects.Furniture
{
    public class ContainerObject : FurnitureObject, IUsableObject
    {
        private const string SaveKeyInventory = "Inventory";

        private readonly Inventory inventory;

        public ContainerObject(SaveData data) 
            : base(data)
        {
            inventory = data.GetObject<Inventory>(SaveKeyInventory);
        }

        public ContainerObject(ContainerObjectConfiguration configuration, int level) 
            : base(configuration)
        {
            var lootLevel = level + configuration.LootLevelIncrement;
            var loot = new TreasureLootGenerator(lootLevel, configuration.ContainerType).GenerateLoot();

            inventory = new Inventory(loot);
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyInventory, inventory);
            return data;
        }

        public void Use(CurrentGame.GameCore<Player> game, Point position)
        {
            game.Journal.Write(new ContainerOpenMessage(Name));
            DialogsManager.Provider.OpenInventoryDialog(Name, inventory);
        }

        public override void OnDeath(Point position)
        {
            base.OnDeath(position);

            foreach (var stack in inventory.Stacks)
            {
                foreach (var item in stack.Items)
                {
                    CurrentGame.Map.AddObject(position, item);
                }
            }
        }
    }

    public class ContainerObjectConfiguration : FurnitureObjectConfiguration
    {
        public string ContainerType { get; set; }

        public int LootLevelIncrement { get; set; }
    }
}