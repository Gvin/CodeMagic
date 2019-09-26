using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Objects.IceObjects
{
    public abstract class AbstractIceObject : IIceObject
    {
        private const int MaxSlideDistance = 3;
        private const int SlideSpeedDamageMultiplier = 1;

        protected readonly ILiquidConfiguration Configuration;
        private int volume;

        protected AbstractIceObject(int volume, string liquidType)
        {
            Configuration = ConfigurationManager.GetLiquidConfiguration(liquidType);
            if (Configuration == null)
                throw new ApplicationException($"Unable to find liquid configuration for liquid type \"{liquidType}\".");

            this.volume = volume;
        }

        public ObjectSize Size => ObjectSize.Huge;

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public abstract string Type { get; }

        public int Volume
        {
            get => volume;
            set
            {
                if (value < 0)
                {
                    volume = 0;
                    return;
                }

                volume = value;
            }
        }

        protected abstract int MinVolumeForEffect { get; }

        public abstract string Name { get; }

        public bool BlocksMovement => false;

        public bool BlocksProjectiles => false;

        public bool BlocksAttack => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public bool SupportsSlide => Volume >= MinVolumeForEffect;

        public ZIndex ZIndex => ZIndex.FloorCover;

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            var cell = map.GetCell(position);
            if (Volume <= 0)
                map.RemoveObject(position, this);

            if (cell.Temperature > Configuration.FreezingPoint)
            {
                ProcessMelting(map, position, cell);
            }
        }

        private void ProcessMelting(IAreaMap map, Point position, IAreaMapCell cell)
        {
            var excessTemperature = cell.Temperature - Configuration.FreezingPoint;
            var volumeToLowerTemp = (int)Math.Floor(excessTemperature * Configuration.MeltingTemperatureMultiplier);
            var volumeToMelt = Math.Min(volumeToLowerTemp, Volume);
            var heatLoss = (int)Math.Floor(volumeToMelt * Configuration.MeltingTemperatureMultiplier);

            cell.Temperature -= heatLoss;
            Volume -= volumeToMelt;

            map.AddObject(position, CreateLiquid(volumeToMelt));
        }

        protected abstract ILiquidObject CreateLiquid(int volume);

        public bool Updated { get; set; }

        public Point ProcessStepOn(IAreaMap map, IJournal journal, Point position, ICreatureObject target, Point initialTargetPosition)
        {
            if (Volume < MinVolumeForEffect)
                return position;

            var direction = Point.GetAdjustedPointRelativeDirection(initialTargetPosition, position);
            if (!direction.HasValue)
                throw new ApplicationException("Unable to get movement direction.");

            var remainingSpeed = TryMoveTarget(map, journal, position, target, direction.Value, out var newPosition, out var blockCell);
            if (remainingSpeed <= 0)
                return newPosition;

            var damage = remainingSpeed * SlideSpeedDamageMultiplier;
            target.Damage(journal, damage, Element.Blunt);
            journal.Write(new EnvironmentDamageMessage(target, damage, Element.Blunt));

            var blockObject = blockCell.GetBiggestDestroyable();
            if (blockObject != null)
            {
                blockObject.Damage(journal, damage, Element.Blunt);
                journal.Write(new EnvironmentDamageMessage(blockObject, damage, Element.Blunt));
            }
            return newPosition;
        }

        private int TryMoveTarget(IAreaMap map, IJournal journal, Point position, ICreatureObject target, Direction direction, out Point newPosition, out IAreaMapCell blockCell)
        {
            newPosition = position;
            blockCell = null;
            for (var remainingSpeed = MaxSlideDistance; remainingSpeed > 0; remainingSpeed--)
            {
                var nextPosition = Point.GetPointInDirection(newPosition, direction);
                var nextCell = map.TryGetCell(nextPosition);
                if (nextCell == null)
                    return 0;
                
                if (nextCell.BlocksMovement)
                {
                    blockCell = nextCell;
                    return remainingSpeed;
                }

                var movementResult = MovementHelper.MoveObject(target, map, journal, newPosition, nextPosition, false);
                if (!movementResult.Success)
                    return remainingSpeed;

                newPosition = nextPosition;

                if (!CellContainsIce(nextCell))
                    return 0;
            }

            return 0;
        }

        private bool CellContainsIce(IAreaMapCell cell)
        {
            var iceObject = cell.Objects.OfType<IIceObject>().FirstOrDefault();
            if (iceObject == null)
                return false;

            return iceObject.Volume >= MinVolumeForEffect;
        }

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }
    }
}