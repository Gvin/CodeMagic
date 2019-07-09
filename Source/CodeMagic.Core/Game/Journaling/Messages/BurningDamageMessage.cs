using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class BurningDamageMessage : IJournalMessage
    {
        public BurningDamageMessage(IDestroyableObject o, int damage)
        {
            Object = o;
            Damage = damage;
        }

        public IDestroyableObject Object { get; }

        public int Damage { get; }
    }
}