using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public abstract class EquipableItem : Item, IEquipableItem, ILightSource, ILightObject
    {
        private const string SaveKeyBonuses = "Bonuses";
        private const string SaveKeyStatBonuses = "StatBonuses";
        private const string SaveKeyLightPower = "LightPower";

        protected EquipableItem(SaveData data) : base(data)
        {
            Bonuses = data.GetObject<DictionarySaveable>(SaveKeyBonuses).Data.ToDictionary(
                pair => (EquipableBonusType) int.Parse((string) pair.Key), pair => int.Parse((string) pair.Value));
            StatBonuses = data.GetObject<DictionarySaveable>(SaveKeyStatBonuses).Data.ToDictionary(
                pair => (PlayerStats)int.Parse((string)pair.Key), pair => int.Parse((string)pair.Value));

            LightPower = (LightLevel) data.GetIntValue(SaveKeyLightPower);
            IsLightOn = LightPower > LightLevel.Darkness;
        }

        protected EquipableItem(EquipableItemConfiguration configuration) 
            : base(configuration)
        {
            Bonuses = configuration.Bonuses.ToDictionary(pair => pair.Key, pair => pair.Value);
            StatBonuses = configuration.StatBonuses.ToDictionary(pair => pair.Key, pair => pair.Value);

            LightPower = configuration.LightPower;
            IsLightOn = LightPower > LightLevel.Darkness;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyBonuses, new DictionarySaveable(Bonuses.ToDictionary(pair => (object)(int)pair.Key, pair => (object)pair.Value)));
            data.Add(SaveKeyStatBonuses, new DictionarySaveable(StatBonuses.ToDictionary(pair => (object)(int)pair.Key, pair => (object)pair.Value)));
            data.Add(SaveKeyLightPower, (int) LightPower);
            return data;
        }

        protected Dictionary<EquipableBonusType, int> Bonuses { get; }

        protected Dictionary<PlayerStats, int> StatBonuses { get; }

        public int GetBonus(EquipableBonusType bonusType)
        {
            if (!Bonuses.ContainsKey(bonusType))
                return 0;

            return Bonuses[bonusType];
        }

        public int GetStatBonus(PlayerStats statType)
        {
            if (!StatBonuses.ContainsKey(statType))
                return 0;

            return StatBonuses[statType];
        }

        public bool IsLightOn { get; }

        public LightLevel LightPower { get; }

        public ILightSource[] LightSources => new ILightSource[] { this };
    }

    public class EquipableItemConfiguration : ItemConfiguration
    {
        public EquipableItemConfiguration()
        {
            LightPower = LightLevel.Darkness;
            Bonuses = new Dictionary<EquipableBonusType, int>();
            StatBonuses = new Dictionary<PlayerStats, int>();
        }

        public Dictionary<EquipableBonusType, int> Bonuses { get; set; }

        public Dictionary<PlayerStats, int> StatBonuses { get; set; }

        public LightLevel LightPower { get; set; }
    }

    public enum EquipableBonusType
    {
        Health,
        Mana,
        ManaRegeneration
    }
}