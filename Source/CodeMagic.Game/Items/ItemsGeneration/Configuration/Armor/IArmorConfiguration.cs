using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor
{
    public interface IArmorConfiguration
    {
        IArmorPieceConfiguration[] ChestConfiguration { get; }

        IArmorPieceConfiguration[] LeggingsConfiguration { get; }

        IArmorPieceConfiguration[] HelmetConfiguration { get; }

        IDescriptionConfiguration DescriptionConfiguration { get; }
    }
}