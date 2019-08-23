using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Core.Items
{
    public class Inventory
    {
        public event EventHandler<ItemEventArgs> ItemAdded;
        public event EventHandler<ItemEventArgs> ItemRemoved; 

        private readonly List<InventoryStack> stacks;

        public Inventory()
        {
            stacks = new List<InventoryStack>();
        }

        public Inventory(IEnumerable<IItem> items)
        {
            stacks = new List<InventoryStack>();
            AddItems(items);
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

        public void RemoveItem(IItem item)
        {
            lock (stacks)
            {
                var existingStack = GetItemStack(item);
                if (existingStack == null)
                {
                    throw new InvalidOperationException($"Item {item.Key} not found in inventory.");
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

        public int GetItemsCount(IItem item)
        {
            return stacks.Where(stack => string.Equals(stack.TopItem.Key, item.Key)).Sum(stack => stack.Count);
        }

        public bool GetIfItemExists(IItem item)
        {
            return stacks.Any(stack => string.Equals(stack.TopItem.Key, item.Key));
        }

        public IItem GetItem(string itemKey)
        {
            return stacks.FirstOrDefault(stack => string.Equals(stack.TopItem.Key, itemKey))?.TopItem;
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

    public class InventoryStack
    {
        private readonly List<IItem> items;

        public InventoryStack(IItem item)
        {
            items = new List<IItem> {item};
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