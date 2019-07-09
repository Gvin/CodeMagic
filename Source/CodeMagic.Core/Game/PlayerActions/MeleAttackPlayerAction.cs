using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game.PlayerActions
{
    public class MeleAttackPlayerAction : IPlayerAction
    {
        public bool Perform(IPlayer player, Point playerPosition, IAreaMap map, Journal journal)
        {
            var targetPoint = Point.GetAdjustedPoint(playerPosition, player.Direction);
            if (!map.ContainsCell(targetPoint))
                return true;

            var targetCell = map.GetCell(targetPoint);
            var possibleTargets = targetCell.Objects.OfType<IDestroyableObject>().ToArray();

            var target = GetAttackTarget(possibleTargets);
            if (target == null)
                return true;

            var damage = GetDamage(player);
            target.Health -= damage;
            journal.Write(new DealDamageMessage(player, target, damage));
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