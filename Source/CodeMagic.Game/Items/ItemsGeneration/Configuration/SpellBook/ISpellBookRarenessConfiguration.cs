using CodeMagic.Core.Items;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.SpellBook
{
    public interface ISpellBookRarenessConfiguration
    {
        ItemRareness Rareness { get; }

        int MinBonuses { get; }

        int MaxBonuses { get; }

        int MinSpells { get; }

        int MaxSpells { get; }

        string[] Description { get; }
    }
}