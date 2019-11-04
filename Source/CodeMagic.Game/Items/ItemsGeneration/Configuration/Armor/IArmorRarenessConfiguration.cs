using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor
{
    public interface IArmorRarenessConfiguration
    {
        ItemRareness Rareness { get; }

        IElementConfiguration[] Protection { get; }

        int MinBonuses { get; }

        int MaxBonuses { get; }

        ItemMaterial[] Materials { get; }
    }
}