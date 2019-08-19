using CodeMagic.Core.Game;

namespace CodeMagic.Core.Configuration.Monsters
{
    public interface IMonsterProtectionConfiguration
    {
        Element Element { get; }

        int Value { get; }
    }
}