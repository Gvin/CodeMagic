using CodeMagic.Game.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class WeaponEquipedMessage : SelfDescribingJournalMessage
    {
        private readonly WeaponItem weapon;
        private readonly bool isRightHand;

        public WeaponEquipedMessage(WeaponItem weapon, bool isRightHand)
        {
            this.weapon = weapon;
            this.isRightHand = isRightHand;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{PlayerName} equiped [",
                GetItemNameText(weapon),
                $"] to your {GetHandName(isRightHand)} hand"
            };
        }

        private static string GetHandName(bool right)
        {
            return right ? "right" : "left";
        }
    }
}