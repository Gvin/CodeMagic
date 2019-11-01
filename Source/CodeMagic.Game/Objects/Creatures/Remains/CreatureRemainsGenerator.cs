using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Objects.Creatures.Remains
{
    public class CreatureRemainsGenerator
    {
        public IMapObject GenerateRemains(NonPlayableCreatureObject creature)
        {
            return new CreatureRemains(creature.RemainsType);
        }
    }
}