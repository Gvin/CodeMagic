using System.Linq;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Game.Items
{
    public class Pickaxe : WeaponItemImpl
    {
        public Pickaxe(PickaxeConfiguration configuration)
            : base(configuration)
        {
            PickaxePower = configuration.PickaxePower;
        }

        public int PickaxePower { get; }

        protected override StyledLine[] GetCharacteristicsDescription(IPlayer player)
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

    public class PickaxeConfiguration : WeaponItemImplConfiguration
    {
        public int PickaxePower { get; set; }
    }
}