using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Objects.Creatures.Remains
{
    public class CreatureRemainsGenerator
    {
        public IMapObject GenerateRemains(NonPlayableCreatureObject creature)
        {
            var remainsConfig = GetRemainsConfiguration(creature);
            var remainsObject = new CreatureRemainsObjectImpl(remainsConfig);
            return remainsObject;
        }

        private CreatureRemainsObjectConfiguration GetRemainsConfiguration(NonPlayableCreatureObject creature)
        {
            return new CreatureRemainsObjectConfiguration(creature.RemainsType);
        }
    }
}