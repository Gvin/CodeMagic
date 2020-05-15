using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.JournalMessages
{
    public class AttackDodgedMessage : SelfDescribingJournalMessage
    {
        private readonly IDestroyableObject target;
        private readonly ICreatureObject source;

        public AttackDodgedMessage(ICreatureObject source, IDestroyableObject target)
        {
            this.source = source;
            this.target = target;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                GetMapObjectName(target),
                " dodged attack coming from ",
                GetMapObjectName(source)
            };
        }
    }
}