using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Game.Items
{
    public abstract class EquipableItem : Item, IEquipableItem, ILightSource, ILightObject
    {
        private const string SaveKeyHealthBonus = "HealthBonus";
        private const string SaveKeyManaBonus = "ManaBonus";
        private const string SaveKeyManaRegenerationBonus = "ManaRegenerationBonus";
        private const string SaveKeyLightPower = "LightPower";

        protected EquipableItem(SaveData data) : base(data)
        {
            HealthBonus = data.GetIntValue(SaveKeyHealthBonus);
            ManaBonus = data.GetIntValue(SaveKeyManaBonus);
            ManaRegenerationBonus = data.GetIntValue(SaveKeyManaRegenerationBonus);

            LightPower = (LightLevel) data.GetIntValue(SaveKeyLightPower);
            IsLightOn = LightPower > LightLevel.Darkness;
        }

        protected EquipableItem(EquipableItemConfiguration configuration) 
            : base(configuration)
        {
            HealthBonus = configuration.HealthBonus;
            ManaBonus = configuration.ManaBonus;
            ManaRegenerationBonus = configuration.ManaRegenerationBonus;

            LightPower = configuration.LightPower;
            IsLightOn = LightPower > LightLevel.Darkness;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyHealthBonus, HealthBonus);
            data.Add(SaveKeyManaBonus, ManaBonus);
            data.Add(SaveKeyManaRegenerationBonus, ManaRegenerationBonus);
            data.Add(SaveKeyLightPower, (int) LightPower);
            return data;
        }

        public int HealthBonus { get; }

        public int ManaBonus { get; }

        public int ManaRegenerationBonus { get; }

        public bool IsLightOn { get; }

        public LightLevel LightPower { get; }

        public ILightSource[] LightSources => new ILightSource[] { this };
    }

    public class EquipableItemConfiguration : ItemConfiguration
    {
        public EquipableItemConfiguration()
        {
            LightPower = LightLevel.Darkness;
        }

        public int HealthBonus { get; set; }

        public int ManaBonus { get; set; }

        public int ManaRegenerationBonus { get; set; }

        public LightLevel LightPower { get; set; }
    }
}