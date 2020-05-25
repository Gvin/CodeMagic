using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Items
{
    public class Inventory : ISaveable
    {
        private const string SaveKeyStacks = "Stacks";

        public event EventHandler<ItemEventArgs> ItemAdded;
        public event EventHandler<ItemEventArgs> ItemRemoved; 

        private readonly List<InventoryStack> stacks;

        public Inventory(SaveData data)
        {
            stacks = data.GetObjectsCollection<InventoryStack>(SaveKeyStacks).ToList();

            foreach (var inventoryStack in stacks)
            {
                foreach (var item in inventoryStack.Items.OfType<IDecayItem>())
                {
                    item.Decayed += Item_Decayed;
                }
            }
        }

        public Inventory()
        {
            stacks = new List<InventoryStack>();
        }

        public Inventory(IEnumerable<IItem> items)
        {
            stacks = new List<InventoryStack>();
            AddItems(items);
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyStacks, stacks.ToArray()}
            });
        }

        public void AddItems(IEnumerable<IItem> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public void AddItem(IItem item)
        {
            lock (stacks)
            {
                if (item is IDecayItem decayItem)
                {
                    decayItem.Decayed += Item_Decayed;
                }

                if (!item.Stackable)
                {
                    stacks.Add(new InventoryStack(item));
                    return;
                }

                var existingStack = GetItemStack(item);
                if (existingStack != null)
                {
                    existingStack.Add(item);
                }
                else
                {
                    stacks.Add(new InventoryStack(item));
                }

                ItemAdded?.Invoke(this, new ItemEventArgs(item));
            }
        }

        private void Item_Decayed(object sender, EventArgs args)
        {
            var item = (IItem) sender;
            RemoveItem(item);
        }

        public void Update()
        {
            foreach (var stack in stacks.ToArray())
            {
                foreach (var item in stack.Items.OfType<IDecayItem>().ToArray())
                {
                    item.Update();
                }
            }
        }

        public void RemoveItem(IItem item)
        {
            lock (stacks)
            {
                var existingStack = GetItemStack(item);
                if (existingStack == null)
                {
                    throw new InvalidOperationException($"Item {item.Key} not found in inventory.");
                }

                if (item is IDecayItem decayItem)
                {
                    decayItem.Decayed -= Item_Decayed;
                }

                existingStack.Remove(item);
                if (existingStack.Count == 0)
                {
                    stacks.Remove(existingStack);
                }

                ItemRemoved?.Invoke(this, new ItemEventArgs(item));
            }
        }

        public int ItemsCount => stacks.Sum(stack => stack.Count);

        public InventoryStack[] Stacks => stacks.ToArray();

        public int GetWeight()
        {
            return stacks.Sum(stack => stack.Weight);
        }

        public IItem GetItem(string itemKey)
        {
            return stacks.FirstOrDefault(stack => string.Equals(stack.TopItem.Key, itemKey))?.TopItem;
        }

        public IItem GetItemById(string itemId)
        {
            return stacks.FirstOrDefault(stack => stack.Items.Any(item => string.Equals(item.Id, itemId)))?.Items
                .FirstOrDefault(item => string.Equals(item.Id, itemId));
        }

        public bool Contains(string itemKey)
        {
            return stacks.Any(stack => string.Equals(stack.TopItem.Key, itemKey));
        }

        private InventoryStack GetItemStack(IItem item)
        {
            return stacks.FirstOrDefault(stack => stack.CheckItemMatches(item));
        }
    }

    public class ItemEventArgs : EventArgs
    {
        public ItemEventArgs(IItem item)
        {
            Item = item;
        }

        public IItem Item { get; }
    }

    public class InventoryStack : ISaveable
    {
        private const string SaveKeyItems = "Items";

        private readonly List<IItem> items;

        public InventoryStack(SaveData data)
        {
            items = data.GetObjectsCollection(SaveKeyItems).Cast<IItem>().ToList();
        }

        public InventoryStack(IItem item)
        {
            items = new List<IItem> {item};
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyItems, items}
            });
        }

        public void Add(IItem item)
        {
            if (!item.Stackable)
                throw new ArgumentException("Stackable item expected.");
            if (!CheckItemMatches(item))
                throw new ArgumentException("Item doesn't match stack.");

            items.Add(item);
        }

        public void Remove(IItem item)
        {
            if (!items.Contains(item))
                throw new ArgumentException("Item not found in stack.");

            items.Remove(item);
        }

        public IItem[] Items => items.ToArray();

        public IItem TopItem => items.Last();

        public int Weight => items.Sum(item => item.Weight);

        public bool CheckItemMatches(IItem item)
        {
            return item.Equals(items.First());
        }

        public int Count => items.Count;
    }
}