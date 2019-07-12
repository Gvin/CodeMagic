using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Area.Liquids;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects.DecorativeObjects
{
    public class IceObject : IMapObject, IDynamicObject, IStepReactionObject
    {
        private const int MaxSlideDistance = 3;
        private const int SlideSpeedDamageMultiplier = 1;
        public const int MinVolumeForEffect = 50;

        public IceObject(int volume)
        {
            Volume = volume;
        }

        public int Volume { get; set; }

        public string Name => "Ice";

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public void Update(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            if (cell.Environment.Temperature > WaterLiquid.FreezingPoint)
            {
                cell.Liquids.AddLiquid(new WaterLiquid(Volume));
                cell.Objects.Remove(this);
            }
        }

        public bool Updated { get; set; }

        public Point ProcessStepOn(IGameCore game, Point position, ICreatureObject target, Point initialTargetPosition)
        {
            if (Volume < MinVolumeForEffect)
                return position;

            var direction = Point.GetAdjustedPointRelativeDirection(initialTargetPosition, position);
            if (!direction.HasValue)
                throw new ApplicationException("Unable to get movement direction.");

            var remainingSpeed = TryMoveTarget(game, position, target, direction.Value, out var newPosition, out var blockCell);
            if (remainingSpeed <= 0)
                return newPosition;

            var damage = remainingSpeed * SlideSpeedDamageMultiplier;
            target.Damage(damage);
            game.Journal.Write(new EnvironmentDamageMessage(target, damage));

            var blockObject = blockCell.GetBiggestDestroyable();
            if (blockObject != null)
            {
                blockObject.Damage(damage);
                game.Journal.Write(new EnvironmentDamageMessage(blockObject, damage));
            }
            return newPosition;
        }

        private int TryMoveTarget(IGameCore game, Point position, ICreatureObject target, Direction direction, out Point newPosition, out AreaMapCell blockCell)
        {
            newPosition = position;
            blockCell = null;
            for (var remainingSpeed = MaxSlideDistance; remainingSpeed > 0; remainingSpeed--)
            {
                var nextPosition = Point.GetPointInDirection(newPosition, direction);
                if (!game.Map.ContainsCell(nextPosition))
                    return 0;
                var nextCell = game.Map.GetCell(nextPosition);
                if (nextCell.BlocksMovement)
                {
                    blockCell = nextCell;
                    return remainingSpeed;
                }

                var movementResult = MovementHelper.MoveObject(target, game, newPosition, nextPosition, false);
                if (!movementResult.Success)
                    return remainingSpeed;

                newPosition = nextPosition;

                if (!CellContainsIce(nextCell))
                    return 0;
            }

            return 0;
        }

        private bool CellContainsIce(AreaMapCell cell)
        {
            var iceObject = cell.Objects.OfType<IceObject>().FirstOrDefault();
            if (iceObject == null)
                return false;

            return iceObject.Volume >= MinVolumeForEffect;
        }
    }
}