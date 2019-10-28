using CodeMagic.Core.Game;

namespace CodeMagic.Game.Configuration.Monsters
{
    public interface IMonsterDamageConfiguration
    {
        Element Element { get; }

        int MinValue { get; }

        int MaxValue { get; }
    }
}