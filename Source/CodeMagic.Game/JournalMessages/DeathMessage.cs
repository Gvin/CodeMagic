using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.JournalMessages
{
    public class DeathMessage : SelfDescribingJournalMessage
    {
        private readonly IMapObject obj;

        public DeathMessage(IMapObject @object)
        {
            obj = @object;
        }

        public override StyledLine GetDescription()
        {
            var deathMessage = obj is ICreatureObject ? "died" : "destroyed";
            return new StyledLine {$"{GetMapObjectName(obj)} {deathMessage}"};
        }
    }
}