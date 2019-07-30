using System;
using System.Linq;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Game.PlayerActions
{
    public class MeleAttackPlayerAction : IPlayerAction
    {
        public bool Perform(IPlayer player, Point playerPosition, IGameCore game, out Point newPosition)
        {
            newPosition = playerPosition;
            var targetPoint = Point.GetPointInDirection(playerPosition, player.Direction);
            if (!game.Map.ContainsCell(targetPoint))
                return true;

            var targetCell = game.Map.GetCell(targetPoint);
            var possibleTargets = targetCell.Objects.OfType<IDestroyableObject>().ToArray();

            var target = GetAttackTarget(possibleTargets);
            if (target == null)
                return true;

            var hitChance = GetHitChance(player);
            if (!RandomHelper.CheckChance(hitChance))
            {
                game.Journal.Write(new AttackMissMessage(player, target));
                return true;
            }

            var damage = GetDamage(player);
            target.Damage(damage);
            game.Journal.Write(new DealDamageMessage(player, target, damage));
            return true;
        }

        private int GetHitChance(IPlayer player)
        {
            if (player.Statuses.Contains(BlindObjectStatus.StatusType))
            {
                return (int) Math.Round(player.Equipment.Weapon.HitChance * BlindObjectStatus.HitChanceMultiplier);
            }

            return player.Equipment.Weapon.HitChance;
        }

        private int GetDamage(IPlayer player)
        {
            var min = player.Equipment.Weapon.MinDamage;
            var max = player.Equipment.Weapon.MaxDamage;
            return RandomHelper.GetRandomValue(min, max);
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