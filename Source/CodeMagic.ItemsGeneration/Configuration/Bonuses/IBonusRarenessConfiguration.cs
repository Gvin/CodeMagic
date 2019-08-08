using CodeMagic.Core.Items;

namespace CodeMagic.ItemsGeneration.Configuration.Bonuses
{
    public interface IBonusRarenessConfiguration
    {
        ItemRareness Rareness { get; }

        IBonusConfiguration[] Bonuses { get; }
    }
}