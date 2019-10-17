using System;
using System.Linq;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Monsters;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Objects.Creatures.NonPlayable;

namespace CodeMagic.MapGeneration.Dungeon
{
    public class DungeonMonstersGenerator
    {
        private readonly IMonstersConfiguration configuration;

        public DungeonMonstersGenerator()
        {
            configuration = ConfigurationManager.Current.Monsters;
        }

        public INonPlayableCreatureObject GenerateRandomMonster(int level)
        {
            var possibleMonsters = GetPossibleMonsters(level);
            var randomMonster = SelectMonster(possibleMonsters, level);
            return CreateMonster(randomMonster);
        }

        private IMonsterConfiguration SelectMonster(IMonsterConfiguration[] possibleMonsters, int level)
        {
            var topRate = possibleMonsters
                .Select(conf => conf.SpawnConfiguration.FirstOrDefault(spawnConf => spawnConf.Level == level))
                .Where(spawnConf => spawnConf != null)
                .Sum(spawnConf => spawnConf.Rate);

            var value = RandomHelper.GetRandomValue(1, topRate);

            var bottomLine = 0;
            foreach (var possibleMonster in possibleMonsters)
            {
                var spawnConfig = possibleMonster.SpawnConfiguration.FirstOrDefault(conf => conf.Level == level);
                if (spawnConfig == null)
                    continue;

                if (value > bottomLine && value <= spawnConfig.Rate + bottomLine)
                {
                    return possibleMonster;
                }

                bottomLine += spawnConfig.Rate;
            }

            throw new ApplicationException("Unable to generate monster. Invalid spawn configuration.");
        }

        private IMonsterConfiguration[] GetPossibleMonsters(int level)
        {
            var monstersByLevel = configuration.Monsters
                .Where(monster => monster.SpawnConfiguration.Any(spawnConf => spawnConf.Level == level))
                .ToArray();
            if (monstersByLevel.Any())
            {
                return monstersByLevel;
            }

            return configuration.Monsters
                .OrderByDescending(conf => conf.SpawnConfiguration.Sum(spawnConf => spawnConf.Level))
                .Take(3)
                .ToArray();
        }

        private INonPlayableCreatureObject CreateMonster(IMonsterConfiguration config)
        {
            var monsterData = new MonsterCreatureImplConfiguration(config);
            return new MonsterCreatureImpl(monsterData);
        }
    }
}