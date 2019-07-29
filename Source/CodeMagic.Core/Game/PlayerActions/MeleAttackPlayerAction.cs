using System.Linq;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;

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

            var damage = GetDamage(player);
            target.Damage(damage);
            game.Journal.Write(new DealDamageMessage(player, target, damage));
            return true;
        }

        private int GetDamage(IPlayer player)
        {
            var min = player.Equipment.MinDamage;
            var max = player.Equipment.MaxDamage;
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