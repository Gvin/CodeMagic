using System;
using System.Linq;
using CodeMagic.Game.Configuration;

namespace CodeMagic.Game.Objects.Creatures.Loot
{
    public class TreasureLootGenerator : ChancesLootGenerator
    {
        public TreasureLootGenerator(int level, string containerType) 
            : base(GetConfiguration(level, containerType))
        {
        }

        private static ILootConfiguration GetConfiguration(int level, string containerType)
        {
            var levelConfig = ConfigurationManager.Current.Treasure.Levels
                .FirstOrDefault(range =>
                level >= range.StartLevel && level <= range.EndLevel);

            if (levelConfig == null)
                throw new ApplicationException($"Unable to find treasure configuration for level {level}.");

            if (!levelConfig.Loot.ContainsKey(containerType))
                throw new ApplicationException($"Unable to find loot for container {containerType} on level {level}.");

            return levelConfig.Loot[containerType];
        }
    }
}