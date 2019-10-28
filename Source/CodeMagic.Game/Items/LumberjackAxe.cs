using System.Linq;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public class LumberjackAxe : WeaponItem
    {
        public LumberjackAxe(LumberjackAxeConfiguration configuration) 
            : base(configuration)
        {
            LumberjackPower = configuration.LumberjackPower;
        }

        public int LumberjackPower { get; }

        protected override StyledLine[] GetCharacteristicsDescription(Player player)
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

    public class LumberjackAxeConfiguration : WeaponItemConfiguration
    {
        public int LumberjackPower { get; set; }
    }
}