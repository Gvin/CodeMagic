using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon
{
    public interface IWeaponRarenessConfiguration
    {
        ItemRareness Rareness { get; }

        IElementConfiguration[] Damage { get; }

        int MinMaxDamageDifference { get; }

        int MinHitChance { get; }

        int MaxHitChance { get; }

        int MinBonuses { get; }

        int MaxBonuses { get; }

        ItemMaterial[] Materials { get; }
    }
}