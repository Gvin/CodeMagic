namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class HungerDecreasedMessage : IJournalMessage
    {
        public HungerDecreasedMessage(int decreaseValue)
        {
            DecreaseValue = decreaseValue;
        }

        public int DecreaseValue { get; }
    }
}