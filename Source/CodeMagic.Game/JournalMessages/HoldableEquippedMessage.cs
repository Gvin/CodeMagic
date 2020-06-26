using CodeMagic.Game.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class HoldableEquippedMessage : SelfDescribingJournalMessage
    {
        private readonly IHoldableItem holdable;
        private readonly bool isRightHand;

        public HoldableEquippedMessage(IHoldableItem holdable, bool isRightHand)
        {
            this.holdable = holdable;
            this.isRightHand = isRightHand;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} equiped [",
                GetItemNameText(holdable),
                $"] to your {GetHandName(isRightHand)} hand"
            };
        }

        private static string GetHandName(bool right)
        {
            return right ? "right" : "left";
        }
    }
}