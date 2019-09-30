using CodeMagic.Core.Items;

namespace CodeMagic.ItemsGeneration.Configuration.Tool
{
    public interface IToolRarenessConfiguration
    {
        ItemRareness Rareness { get; }

        IElementConfiguration[] Damage { get; }

        int MinMaxDamageDifference { get; }

        int MinHitChance { get; }

        int MaxHitChance { get; }

        ItemMaterial[] Materials { get; }

        int MaxToolPower { get; }

        int MinToolPower { get; }
    }
}