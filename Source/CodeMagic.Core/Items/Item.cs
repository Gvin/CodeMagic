using System;
using System.Collections.Generic;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Items
{
    public abstract class Item : MapObjectBase, IItem
    {
        private const string SaveKeyId = "Id";
        private const string SaveKeyKey = "Key";
        private const string SaveKeyRareness = "Rareness";
        private const string SaveKeyWeight = "Weight";

        protected Item(SaveData data)
            : base(data)
        {
            Id = data.GetStringValue(SaveKeyId);
            Key = data.GetStringValue(SaveKeyKey);
            Rareness = (ItemRareness)data.GetIntValue(SaveKeyRareness);
            Weight = data.GetIntValue(SaveKeyWeight);
        }

        protected Item(ItemConfiguration configuration)
            : base(configuration.Name)
        {
            Id = Guid.NewGuid().ToString();
            Key = configuration.Key;
            Rareness = configuration.Rareness;
            Weight = configuration.Weight;
        }

        public string Id { get; }

        public virtual string Key { get; }

        public virtual ItemRareness Rareness { get; }

        public virtual int Weight { get; }

        public virtual bool Stackable => true;

        public bool Equals(IItem other)
        {
            if (other == null)
                return false;

            if (Stackable)
            {
                return string.Equals(Key, other.Key);
            }

            return string.Equals(Id, other.Id);
        }

        #region IMapObject Implementation

        public override ZIndex ZIndex => ZIndex.AreaDecoration;

        public override ObjectSize Size => ObjectSize.Small;

        #endregion

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyId, Id);
            data.Add(SaveKeyKey, Key);
            data.Add(SaveKeyWeight, Weight);
            data.Add(SaveKeyRareness, (int) Rareness);
            return data;
        }
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