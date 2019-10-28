using System.Linq;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public class Pickaxe : WeaponItem
    {
        public Pickaxe(PickaxeConfiguration configuration)
            : base(configuration)
        {
            PickaxePower = configuration.PickaxePower;
        }

        public int PickaxePower { get; }

        protected override StyledLine[] GetCharacteristicsDescription(Player player)
        {
            var result = base.GetCharacteristicsDescription(player).ToList();

            var equiped = player.Equipment.Weapon as Pickaxe;

            var powerLine = new StyledLine { "Pickaxe Power: " };
            if (equiped == null || Equals(equiped))
            {
                powerLine.Add(ItemTextHelper.GetValueString(PickaxePower, "%", false));
            }
            else
            {
                powerLine.Add(ItemTextHelper.GetCompareValueString(PickaxePower, equiped.PickaxePower, "%", false));
            }
            result.Add(powerLine);

            return result.ToArray();
        }
    }

    public class PickaxeConfiguration : WeaponItemConfiguration
    {
        public int PickaxePower { get; set; }
    }
}