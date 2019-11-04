namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.SpellBook
{
    public interface ISpellBooksConfiguration
    {
        string Template { get; }

        string[] SymbolImages { get; }

        ISpellBookRarenessConfiguration[] Configuration { get; }

        int Weight { get; }
    }
}