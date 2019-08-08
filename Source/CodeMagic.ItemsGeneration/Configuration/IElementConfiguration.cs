using CodeMagic.Core.Game;

namespace CodeMagic.ItemsGeneration.Configuration
{
    public interface IElementConfiguration
    {
        Element Element { get; }

        int MinValue { get; }

        int MaxValue { get; }
    }
}