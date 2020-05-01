using System.Collections.Generic;

namespace CodeMagic.Game.Configuration.Treasure
{
    public interface ITreasureConfiguration
    {
        ITreasureLevelsConfiguration[] Levels { get; }
    }

    public interface ITreasureLevelsConfiguration
    {
        int StartLevel { get; }

        int EndLevel { get; }

        Dictionary<string, ILootConfiguration> Loot { get; }
    }
}