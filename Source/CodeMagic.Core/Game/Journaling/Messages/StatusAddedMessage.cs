using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class StatusAddedMessage : IJournalMessage
    {
        public StatusAddedMessage(IMapObject target, string statusType)
        {
            Target = target;
            StatusType = statusType;
        }

        public IMapObject Target { get; }

        public string StatusType { get; }
    }
}