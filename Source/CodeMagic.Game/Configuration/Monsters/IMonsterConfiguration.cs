using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Configuration.Monsters
{
    public interface IMonsterConfiguration
    {
        IMonsterSpawnConfiguration SpawnConfiguration { get; }

        string Id { get; }

        string Name { get; }

        IMonsterExperienceConfiguration Experience { get; }

        string LogicPattern { get; }

        string Image { get; }

        ObjectSize Size { get; }

        RemainsType RemainsType { get; }

        RemainsType DamageMarkType { get; }

        IMonsterStatsConfiguration Stats { get; }

        ILootConfiguration Loot { get; }
    }

    public interface IMonsterSpawnConfiguration
    {
        int MinLevel { get; }

        int Force { get; }

        string Group { get; }
    }

    public interface IMonsterExperienceConfiguration
    {
        int Max { get; }

        int Min { get; }
    }
}