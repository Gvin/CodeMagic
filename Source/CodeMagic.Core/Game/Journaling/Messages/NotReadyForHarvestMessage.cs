namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class NotReadyForHarvestMessage : IJournalMessage
    {
        public NotReadyForHarvestMessage(string plantName)
        {
            PlantName = plantName;
        }

        public string PlantName { get; }
    }
}