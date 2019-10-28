using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.Game.Objects.Creatures.Remains
{
    public class CreatureRemainsGenerator
    {
        public IMapObject GenerateRemains(INonPlayableCreatureObject creature)
        {
            var remainsConfig = GetRemainsConfiguration(creature);
            var remainsObject = Injector.Current.Create<ICreatureRemainsObject>(remainsConfig);
            return remainsObject;
        }

        private CreatureRemainsObjectConfiguration GetRemainsConfiguration(INonPlayableCreatureObject creature)
        {
            return new CreatureRemainsObjectConfiguration(creature.RemainsType);
        }
    }
}