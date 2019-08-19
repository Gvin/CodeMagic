using CodeMagic.Core.Game;

namespace CodeMagic.Core.Configuration.Monsters
{
    public interface IMonsterDamageConfiguration
    {
        Element Element { get; }

        int MinValue { get; }

        int MaxValue { get; }
    }
}