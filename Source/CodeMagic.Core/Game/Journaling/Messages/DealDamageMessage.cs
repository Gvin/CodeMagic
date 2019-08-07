using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class DealDamageMessage : IJournalMessage
    {
        public DealDamageMessage(IMapObject source, IMapObject target, int damage, Element element)
        {
            Source = source;
            Target = target;
            Damage = damage;
            Element = element;
        }

        public IMapObject Source { get; }

        public IMapObject Target { get; }

        public int Damage { get; }

        public Element Element { get; }
    }
}