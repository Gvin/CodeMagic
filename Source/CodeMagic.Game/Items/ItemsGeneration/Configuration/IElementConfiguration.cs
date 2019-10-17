using CodeMagic.Core.Game;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration
{
    public interface IElementConfiguration
    {
        Element Element { get; }

        int MinValue { get; }

        int MaxValue { get; }
    }
}