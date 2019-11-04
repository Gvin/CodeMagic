using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Description
{
    public interface IRarenessDescriptionConfiguration
    {
        ItemRareness Rareness { get; }

        string[] Text { get; }
    }
}