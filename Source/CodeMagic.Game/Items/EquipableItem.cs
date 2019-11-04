using CodeMagic.Core.Area;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.Items
{
    public class EquipableItem : Item, IEquipableItem, ILightSource, ILightObject
    {
        public EquipableItem(EquipableItemConfiguration configuration) 
            : base(configuration)
        {
            HealthBonus = configuration.HealthBonus;
            ManaBonus = configuration.ManaBonus;
            ManaRegenerationBonus = configuration.ManaRegenerationBonus;

            IsLightOn = configuration.LightPower > LightLevel.Darkness;
            LightPower = configuration.LightPower;
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