using System.Linq;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Monsters;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Implementations.Objects.Creatures.NonPlayable;

namespace CodeMagic.MapGeneration
{
    public class MonstersGenerator
    {
        private readonly IMonstersConfiguration configuration;

        public MonstersGenerator()
        {
            configuration = ConfigurationManager.Current.Monsters;
        }

        public INonPlayableCreatureObject GenerateRandomMonster(int level)
        {
            var monstersConfig = GetPossibleMonsters(level);
            var randomMonsterConfig = RandomHelper.GetRandomElement(monstersConfig);
            return CreateMonster(randomMonsterConfig);
        }

        private IMonsterConfiguration[] GetPossibleMonsters(int level)
        {
            var monstersByLevel = configuration.Monsters.Where(monster => monster.Levels.Contains(level)).ToArray();
            if (monstersByLevel.Any())
            {
                return monstersByLevel;
            }
            return configuration.Monsters.OrderByDescending(conf => conf.Levels.Sum()).Take(3).ToArray();
        }

        private INonPlayableCreatureObject CreateMonster(IMonsterConfiguration config)
        {
            var monsterData = new MonsterCreatureImplConfiguration(config);
            return new MonsterCreatureImpl(monsterData);
        }
    }
}