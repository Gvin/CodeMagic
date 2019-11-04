using CodeMagic.Core.Game;

namespace CodeMagic.Game.Configuration.Monsters
{
    public interface IMonsterProtectionConfiguration
    {
        Element Element { get; }

        int Value { get; }
    }
}