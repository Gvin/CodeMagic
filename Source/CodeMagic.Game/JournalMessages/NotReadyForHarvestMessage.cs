namespace CodeMagic.Game.JournalMessages
{
    public class NotReadyForHarvestMessage : SelfDescribingJournalMessage
    {
        private readonly string plantName;

        public NotReadyForHarvestMessage(string plantName)
        {
            this.plantName = plantName;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine {$"{plantName} is not ready for harvest"};
        }
    }
}