using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class EquipWeaponPlayerAction : IPlayerAction
    {
        private readonly WeaponItem weapon;
        private readonly bool isRight;

        public EquipWeaponPlayerAction(WeaponItem weapon, bool isRight)
        {
            this.weapon = weapon;
            this.isRight = isRight;
        }


        public bool Perform(out Point newPosition)
        {
            ((CurrentGame.GameCore<Player>)CurrentGame.Game).Player.Equipment.EquipWeapon(weapon, isRight);
            CurrentGame.Journal.Write(new WeaponEquipedMessage(weapon, isRight));

            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}