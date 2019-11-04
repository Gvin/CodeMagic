using CodeMagic.Core.Objects;

namespace CodeMagic.Game.JournalMessages
{
    public class AttackMissMessage : SelfDescribingJournalMessage
    {
        private readonly IMapObject source;
        private readonly IMapObject target;

        public AttackMissMessage(IMapObject source, IMapObject target)
        {
            this.source = source;
            this.target = target;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine {$"{GetMapObjectName(source)} missed {GetMapObjectName(target)}"};
        }
    }
}