using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Configuration.Monsters
{
    public interface IMonsterConfiguration
    {
        IMonsterSpawnConfiguration[] SpawnConfiguration { get; }

        string Type { get; }

        string Id { get; }

        string Name { get; }

        IMonsterExperienceConfiguration Experience { get; }

        string LogicPattern { get; }

        IMonsterImagesConfiguration Images { get; }

        ObjectSize Size { get; }

        RemainsType RemainsType { get; }

        RemainsType DamageMarkType { get; }

        IMonsterStatsConfiguration Stats { get; }

        ILootConfiguration Loot { get; }
    }

    public interface IMonsterSpawnConfiguration
    {
        int Level { get; }

        int Rate { get; }
    }

    public interface IMonsterExperienceConfiguration
    {
        int Max { get; }

        int Min { get; }
    }
}