using System.Drawing;
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

            IsLightOn = configuration.LightPower > LightLevel.Darkness && configuration.LightColor != null;
            LightPower = configuration.LightPower;
            LightColor = configuration.LightColor ?? Color.Black;
        }

        public int HealthBonus { get; }

        public int ManaBonus { get; }

        public int ManaRegenerationBonus { get; }

        public bool IsLightOn { get; }

        public LightLevel LightPower { get; }

        public Color LightColor { get; }

        public ILightSource[] LightSources => new ILightSource[] { this };
    }

    public class EquipableItemConfiguration : ItemConfiguration
    {
        public EquipableItemConfiguration()
        {
            LightPower = LightLevel.Darkness;
            LightColor = null;
        }

        public int HealthBonus { get; set; }

        public int ManaBonus { get; set; }

        public int ManaRegenerationBonus { get; set; }

        public Color? LightColor { get; set; }

        public LightLevel LightPower { get; set; }
    }
}