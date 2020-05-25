using System;
using System.Collections.Generic;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;

namespace CodeMagic.Game.Items
{
    public class DurableItem : EquipableItem, IDecayItem
    {
        private const string SaveKeyMaxDurability = "MaxDurability";
        private const string SaveKeyDurability = "Durability";

        private int durability;

        public event EventHandler Decayed;

        public DurableItem(SaveData data) : base(data)
        {
            MaxDurability = data.GetIntValue(SaveKeyMaxDurability);
            Durability = data.GetIntValue(SaveKeyDurability);
        }

        public DurableItem(DurableItemConfiguration configuration) 
            : base(configuration)
        {
            MaxDurability = configuration.MaxDurability;
            Durability = configuration.Durability ?? MaxDurability;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyMaxDurability, MaxDurability);
            data.Add(SaveKeyDurability, Durability);
            return data;
        }

        public int MaxDurability { get; }

        public int Durability
        {
            get => durability;
            set
            {
                durability = Math.Min(MaxDurability, Math.Max(0, value));

                if (durability == 0)
                {
                    Decayed?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Update()
        {
            // Do nothing.
        }
    }

    public class DurableItemConfiguration : EquipableItemConfiguration
    {
        public int MaxDurability { get; set; }

        public int? Durability { get; set; }
    }
}