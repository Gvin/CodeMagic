using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.JournalMessages
{
    public class DealDamageMessage : SelfDescribingJournalMessage
    {
        private readonly IMapObject source;
        private readonly IMapObject target;
        private readonly int damage;
        private readonly Element element;

        public DealDamageMessage(IMapObject source, IMapObject target, int damage, Element element)
        {
            this.source = source;
            this.target = target;
            this.damage = damage;
            this.element = element;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{GetMapObjectName(source)} dealt ",
                GetDamageText(damage, element),
                $" to {GetMapObjectName(target)}"
            };
        }
    }
}