using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.PlayerActions
{
    public class MeleAttackPlayerAction : IPlayerAction
    {
        private readonly bool useRightHand;

        public MeleAttackPlayerAction(bool useRightHand)
        {
            this.useRightHand = useRightHand;
        }

        public bool Perform(out Point newPosition)
        {
            var game = (CurrentGame.GameCore<Player>)CurrentGame.Game;
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

            if (!RandomHelper.CheckChance(useRightHand ? game.Player.AccuracyRight : game.Player.AccuracyLeft))
            {
                game.Journal.Write(new AttackMissMessage(game.Player, target));
                return true;
            }

            if (RandomHelper.CheckChance(target.DodgeChance))
            {
                game.Journal.Write(new AttackDodgedMessage(game.Player, target));
                return true;
            }

            var damage = GenerateDamage(game.Player);
            foreach (var damageValue in damage)
            {
                target.Damage(targetPoint, damageValue.Value, damageValue.Key);
                game.Journal.Write(new DealDamageMessage(game.Player, target, damageValue.Value, damageValue.Key));
            }

            return true;
        }

        private Dictionary<Element, int> GenerateDamage(Player player)
        {
            var weaponDamage = GenerateWeaponDamage(player);
            var damageMultiplier = 1d + player.DamageBonus/100d;

            foreach (var pair in weaponDamage.ToArray())
            {
                if (pair.Key == Element.Piercing ||
                    pair.Key == Element.Blunt ||
                    pair.Key == Element.Slashing)
                {
                    weaponDamage[pair.Key] = (int) Math.Round(weaponDamage[pair.Key] * damageMultiplier);
                }
            }

            return weaponDamage;
        }

        private Dictionary<Element, int> GenerateWeaponDamage(Player player)
        {
            if (useRightHand)
            {
                return player.Equipment.RightWeapon.GenerateDamage();
            }

            return player.Equipment.LeftWeapon.GenerateDamage();
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