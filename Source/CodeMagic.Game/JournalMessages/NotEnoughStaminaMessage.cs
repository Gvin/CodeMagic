namespace CodeMagic.Game.JournalMessages
{
    public class NotEnoughStaminaMessage : SelfDescribingJournalMessage
    {
        public override StyledLine GetDescription()
        {
            return new StyledLine{$"{PlayerName} have not enough ", new StyledString("Stamina", StaminaColor), " for this action"};
        }
    }
}