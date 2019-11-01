using CodeMagic.Core.Objects;

namespace CodeMagic.Game.JournalMessages
{
    public class StatusAddedMessage : SelfDescribingJournalMessage
    {
        private readonly IMapObject target;
        private readonly string statusType;

        public StatusAddedMessage(IMapObject target, string statusType)
        {
            this.target = target;
            this.statusType = statusType;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine {$"{GetMapObjectName(target)} got [{GetStatusName(statusType)}] status"};
        }
    }
}