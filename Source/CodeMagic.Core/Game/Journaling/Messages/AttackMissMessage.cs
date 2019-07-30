using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class AttackMissMessage : IJournalMessage
    {
        public AttackMissMessage(IMapObject source, IMapObject target)
        {
            Source = source;
            Target = target;
        }

        public IMapObject Source { get; }

        public IMapObject Target { get; }
    }
}