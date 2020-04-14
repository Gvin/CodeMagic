using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Configuration.Monsters
{
    public interface IMonsterConfiguration
    {
        IMonsterSpawnConfiguration[] SpawnConfiguration { get; }

        string Type { get; }

        string Name { get; }

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
}