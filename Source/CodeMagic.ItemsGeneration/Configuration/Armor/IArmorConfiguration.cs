using CodeMagic.ItemsGeneration.Configuration.Description;

namespace CodeMagic.ItemsGeneration.Configuration.Armor
{
    public interface IArmorConfiguration
    {
        IArmorPieceConfiguration[] ChestConfiguration { get; }

        IArmorPieceConfiguration[] LeggingsConfiguration { get; }

        IArmorPieceConfiguration[] HelmetConfiguration { get; }

        IDescriptionConfiguration DescriptionConfiguration { get; }
    }
}