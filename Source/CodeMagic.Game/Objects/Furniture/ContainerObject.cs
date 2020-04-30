using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;

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

        public ContainerObject(ContainerObjectConfiguration configuration) 
            : base(configuration)
        {
            inventory = new Inventory();
            // TODO: Fill inventory with loot
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyInventory, inventory);
            return data;
        }

        public void Use(CurrentGame.GameCore<Player> game, Point position)
        {
            DialogsManager.Provider.OpenInventoryDialog(Name, inventory);
        }
    }

    public class ContainerObjectConfiguration : FurnitureObjectConfiguration
    {
    }
}