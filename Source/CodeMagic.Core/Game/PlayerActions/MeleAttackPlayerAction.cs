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
            if (player.Statuses.Contains(ParalyzedObjectStatus.StatusType))
            {
                game.Journal.Write(new ParalyzedMessage());
                return true;
            }

            var targetPoint = Point.GetPointInDirection(playerPosition, player.Direction);
            var targetCell = game.Map.TryGetCell(targetPoint);
            if (targetCell == null)
                return true;

            var possibleTargets = targetCell.Objects.OfType<IDestroyableObject>().ToArray();

            var target = GetAttackTarget(possibleTargets);
            if (target == null)
                return true;

            if (!RandomHelper.CheckChance(player.HitChance))
            {
                game.Journal.Write(new AttackMissMessage(player, target));
                return true;
            }

            var damage = GetDamage(player);
            target.Damage(damage);
            game.Journal.Write(new DealDamageMessage(player, target, damage));
            return true;
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