using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Core.Items
{
    public class Inventory
    {
        private readonly List<InventoryStack> stacks;

        public Inventory(int maxWeight)
        {
            stacks = new List<InventoryStack>();

            MaxWeight = maxWeight;
        }

        public int MaxWeight { get; set; }

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
                    existingStack.Count++;
                }

                stacks.Add(new InventoryStack(item));
            }
        }

        public void RemoveItem(IItem item, int count)
        {
            if (!item.Stackable)
                throw new InvalidOperationException($"Removing multiple items is supported only by stackable items (tried to remove {item.Key}).");

            var existingStack = GetItemStack(item);
            if (existingStack == null)
            {
                throw new InvalidOperationException($"Item {item.Key} not found in inventory.");
            }

            if (existingStack.Count < count)
            {
                throw new InvalidOperationException($"Unable to remove {count} items of type {item.Key}: only {existingStack.Count} in the inventory.");
            }

            existingStack.Count -= count;
            if (existingStack.Count == 0)
            {
                stacks.Remove(existingStack);
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

                existingStack.Count--;
                if (existingStack.Count == 0)
                {
                    stacks.Remove(existingStack);
                }
            }
        }

        public InventoryStack[] Stacks => stacks.ToArray();

        public int Wight
        {
            get { return stacks.Sum(stack => stack.Count * stack.Item.Weight); }
        }

        public int GetItemsCount(IItem item)
        {
            return stacks.Where(stack => string.Equals(stack.Item.Key, item.Key)).Sum(stack => stack.Count);
        }

        public bool GetIfItemExists(IItem item)
        {
            return stacks.Any(stack => string.Equals(stack.Item.Key, item.Key));
        }

        private InventoryStack GetItemStack(IItem item)
        {
            return stacks.FirstOrDefault(stack => string.Equals(stack.Item.Key, item.Key));
        }
    }

    public class InventoryStack
    {
        public InventoryStack(IItem item)
        {
            Item = item;
            Count = 1;
        }

        public IItem Item { get; }

        public int Count { get; set; }
    }
}