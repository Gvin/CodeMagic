using System.Linq;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Implementations.Items
{
    public class LumberjackAxe : WeaponItemImpl
    {
        public LumberjackAxe(LumberjackAxeConfiguration configuration) 
            : base(configuration)
        {
            LumberjackPower = configuration.LumberjackPower;
        }

        public int LumberjackPower { get; set; }

        protected override StyledLine[] GetCharacteristicsDescription(IPlayer player)
        {
            var result = base.GetCharacteristicsDescription(player).ToList();

            var equiped = player.Equipment.Weapon as LumberjackAxe;

            var lumberPowerLine = new StyledLine {"Lumberjack Power: "};
            if (equiped == null || Equals(equiped))
            {
                lumberPowerLine.Add(ItemTextHelper.GetValueString(LumberjackPower, "%", false));
            }
            else
            {
                lumberPowerLine.Add(ItemTextHelper.GetCompareValueString(LumberjackPower, equiped.LumberjackPower, "%", false));
            }
            result.Add(lumberPowerLine);

            return result.ToArray();
        }
    }

    public class LumberjackAxeConfiguration : WeaponItemImplConfiguration
    {
        public int LumberjackPower { get; set; }
    }
}