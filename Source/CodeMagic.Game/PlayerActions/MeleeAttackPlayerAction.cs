using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.PlayerActions
{
    public class MeleeAttackPlayerAction : PlayerActionBase
    {
        private const int StaminaToAttack = 10;
        private readonly bool useRightHand;

        public MeleeAttackPlayerAction(bool useRightHand)
        {
            this.useRightHand = useRightHand;
        }

        protected override int RestoresStamina => 0;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
        {
            newPosition = game.PlayerPosition;

            if (game.Player.Statuses.Contains(ParalyzedObjectStatus.StatusType))
            {
                game.Journal.Write(new ParalyzedMessage());
                return true;
            }

            if (game.Map.GetCell(game.PlayerPosition).Objects.Any(obj => obj.BlocksAttack))
                return true;

            var targetPoint = Point.GetPointInDirection(game.PlayerPosition, game.Player.Direction);
            var targetCell = game.Map.TryGetCell(targetPoint);
            if (targetCell == null)
                return true;

            if (targetCell.Objects.Any(obj => obj.BlocksAttack))
                return true;

            var possibleTargets = targetCell.Objects.OfType<IDestroyableObject>().ToArray();

            var target = GetAttackTarget(possibleTargets);
            if (target == null)
                return true;

            if (game.Player.Stamina < StaminaToAttack)
            {
                game.Journal.Write(new NotEnoughStaminaMessage());
                return true;
            }

            game.Player.Stamina -= StaminaToAttack;

            var holdableItem = useRightHand ? game.Player.Equipment.RightHandItem : game.Player.Equipment.LeftHandItem;
            if (!(holdableItem is IWeaponItem weapon))
            {
                game.Journal.Write(new CantAttackWithItemMessage(holdableItem));
                return false;
            }

            var accuracy = weapon.Accuracy;
            accuracy += game.Player.AccuracyBonus;
            if (!RandomHelper.CheckChance(accuracy))
            {
                game.Journal.Write(new AttackMissMessage(game.Player, target));
                return true;
            }

            if (RandomHelper.CheckChance(target.DodgeChance))
            {
                game.Journal.Write(new AttackDodgedMessage(game.Player, target));
                return true;
            }

            var attackDirection = Point.GetAdjustedPointRelativeDirection(targetPoint, game.PlayerPosition);
            if (attackDirection == null)
                throw new ApplicationException("Can only attack adjusted target");

            var damage = GenerateDamage(game.Player, weapon);
            foreach (var damageValue in damage)
            {
                target.MeleeDamage(targetPoint, attackDirection.Value, damageValue.Value, damageValue.Key);
                game.Journal.Write(new DealDamageMessage(game.Player, target, damageValue.Value, damageValue.Key));
            }

            return true;
        }

        private Dictionary<Element, int> GenerateDamage(Player player, IWeaponItem weapon)
        {
            var weaponDamage = weapon.GenerateDamage();

            foreach (var pair in weaponDamage.ToArray())
            {
                weaponDamage[pair.Key] = AttackHelper.CalculateDamage(pair.Value, pair.Key, player);
            }

            return weaponDamage;
        }

        private IDestroyableObject GetAttackTarget(IDestroyableObject[] possibleTargets)
        {
            if (possibleTargets.Length == 0)
                return null;

            var bestTarget = possibleTargets.FirstOrDefault(target => target.BlocksMovement);
            if (bestTarget != null)
                return bestTarget;

            return possibleTargets.LastOrDefault();
        }
    }
}