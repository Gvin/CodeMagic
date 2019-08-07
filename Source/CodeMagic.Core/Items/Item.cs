using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Items.Bonuses.Common;

namespace CodeMagic.Core.Items
{
    public class Item : IItem
    {
        protected readonly List<IItemBonus> Bonuses;

        private readonly int weight;

        public Item(ItemConfiguration configuration)
        {
            Key = configuration.Key;
            Name = configuration.Name;
            Rareness = configuration.Rareness;
            weight = configuration.Weight;

            Id = Guid.NewGuid().ToString();

            Bonuses = new List<IItemBonus>();
        }

        public string Id { get; }

        public string Key { get; }

        public string Name { get; }

        public ItemRareness Rareness { get; }

        public int Weight
        {
            get
            {
                var weightDecrease = Bonuses.OfType<ICommonItemBonus>().Sum(bonus => bonus.WeightDecrease);
                return Math.Max(0, weight - weightDecrease);
            }
        }

        public virtual bool Stackable => true;

        public bool Equals(IItem other)
        {
            if (Stackable)
            {
                return string.Equals(Key, other.Key);
            }

            return string.Equals(Id, other.Id);
        }
    }

    public interface IItemBonus
    {
    }

    public class ItemConfiguration
    {
        public ItemConfiguration()
        {
            Rareness = ItemRareness.Trash;
            Weight = 1;
        }

        public string Key { get; set; }

        public string Name { get; set; }

        public ItemRareness Rareness { get; set; }

        public int Weight { get; set; }
    }
}