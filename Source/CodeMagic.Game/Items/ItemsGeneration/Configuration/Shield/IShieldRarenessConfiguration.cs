using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield
{
    public interface IShieldRarenessConfiguration
    {
        ItemRareness Rareness { get; }

        IIntervalConfiguration BlocksDamage { get; }

        IIntervalConfiguration ProtectChance { get; }

        IIntervalConfiguration HitChancePenalty { get; }

        IIntervalConfiguration Bonuses { get; }

        ItemMaterial[] Materials { get; }
    }
}