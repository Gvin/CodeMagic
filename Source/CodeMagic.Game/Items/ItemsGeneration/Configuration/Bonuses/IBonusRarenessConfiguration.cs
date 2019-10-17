using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses
{
    public interface IBonusRarenessConfiguration
    {
        ItemRareness Rareness { get; }

        IBonusConfiguration[] Bonuses { get; }
    }
}