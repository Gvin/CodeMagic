using CodeMagic.Core.Items;

namespace CodeMagic.ItemsGeneration.Configuration.Weapon.Description
{
    public interface IRarenessDescriptionConfiguration
    {
        ItemRareness Rareness { get; }

        string[] Text { get; }
    }
}