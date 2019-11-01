using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Locations;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class MeleAttackPlayerAction : IPlayerAction
    {
        public bool Perform(IGameCore gameObject, out Point newPosition)
        {
            var game = (GameCore<Player>) gameObject;
            newPosition = game.PlayerPosition;
            if (!game.World.CurrentLocation.CanFight())
            {
                game.Journal.Write(new FightNotAllowedMessage());
                return false;
            }

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

            if (!RandomHelper.CheckChance(game.Player.HitChance))
            {
                game.Journal.Write(new AttackMissMessage(game.Player, target));
                return true;
            }

            var damage = game.Player.Equipment.Weapon.GenerateDamage();
            foreach (var damageValue in damage)
            {
                if (target is IResourceObject resourceObject)
                {
                    resourceObject.UseTool(game, game.Player.Equipment.Weapon, damageValue.Value, damageValue.Key);
                }
                else
                {
                    target.Damage(game.Journal, damageValue.Value, damageValue.Key);
                }
                game.Journal.Write(new DealDamageMessage(game.Player, target, damageValue.Value, damageValue.Key));
            }

            return true;
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