using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.JournalMessages
{
    public class ShieldBlockedDamageMessage : SelfDescribingJournalMessage
    {
        private readonly ICreatureObject creature;
        private readonly int damage;
        private readonly Element element;

        public ShieldBlockedDamageMessage(ICreatureObject creature, int damage, Element element)
        {
            this.creature = creature;
            this.damage = damage;
            this.element = element;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{GetMapObjectName(creature)} blocked ",
                GetDamageText(damage, element),
                " with shield"
            };
        }
    }
}