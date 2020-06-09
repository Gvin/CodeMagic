using CodeMagic.Core.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class EquipWeaponPlayerAction : PlayerActionBase
    {
        private readonly WeaponItem weapon;
        private readonly bool isRight;

        public EquipWeaponPlayerAction(WeaponItem weapon, bool isRight)
        {
            this.weapon = weapon;
            this.isRight = isRight;
        }

        protected override int RestoresStamina => 10;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
        {
            game.Player.Equipment.EquipWeapon(weapon, isRight);
            CurrentGame.Journal.Write(new WeaponEquipedMessage(weapon, isRight));

            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}