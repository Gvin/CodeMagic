using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Objects.IceObjects
{
    public abstract class AbstractIce : IIce
    {
        private const int MaxSlideDistance = 3;
        private const int SlideSpeedDamageMultiplier = 1;

        protected readonly ILiquidConfiguration Configuration;
        private int volume;

        protected AbstractIce(int volume, string liquidType)
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
            set => volume = Math.Max(0, value);
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

        public void Update(Point position)
        {
            var cell = CurrentGame.Map.GetCell(position);
            if (Volume <= 0)
                CurrentGame.Map.RemoveObject(position, this);

            if (cell.Temperature() > Configuration.FreezingPoint)
            {
                ProcessMelting(CurrentGame.Map, position, cell);
            }
        }

        private void ProcessMelting(IAreaMap map, Point position, IAreaMapCell cell)
        {
            var excessTemperature = cell.Temperature() - Configuration.FreezingPoint;
            var volumeToLowerTemp = (int)Math.Floor(excessTemperature * Configuration.MeltingTemperatureMultiplier);
            var volumeToMelt = Math.Min(volumeToLowerTemp, Volume);
            var heatLoss = (int)Math.Floor(volumeToMelt * Configuration.MeltingTemperatureMultiplier);

            cell.Environment.Cast().Temperature -= heatLoss;
            Volume -= volumeToMelt;

            map.AddObject(position, CreateLiquid(volumeToMelt));
        }

        protected abstract ILiquid CreateLiquid(int volume);

        public bool Updated { get; set; }

        public Point ProcessStepOn(Point position, ICreatureObject target, Point initialTargetPosition)
        {
            if (Volume < MinVolumeForEffect)
                return position;

            var direction = Point.GetAdjustedPointRelativeDirection(initialTargetPosition, position);
            if (!direction.HasValue)
                throw new ApplicationException("Unable to get movement direction.");

            var remainingSpeed = TryMoveTarget(position, target, direction.Value, out var newPosition, out var blockCell);
            if (remainingSpeed <= 0)
                return newPosition;

            var damage = remainingSpeed * SlideSpeedDamageMultiplier;
            target.Damage(position, damage, Element.Blunt);
            CurrentGame.Journal.Write(new EnvironmentDamageMessage(target, damage, Element.Blunt));

            var blockObject = blockCell.GetBiggestDestroyable();
            if (blockObject != null)
            {
                blockObject.Damage(position, damage, Element.Blunt);
                CurrentGame.Journal.Write(new EnvironmentDamageMessage(blockObject, damage, Element.Blunt));
            }
            return newPosition;
        }

        private int TryMoveTarget(Point position, ICreatureObject target, Direction direction, out Point newPosition, out IAreaMapCell blockCell)
        {
            newPosition = position;
            blockCell = null;
            for (var remainingSpeed = MaxSlideDistance; remainingSpeed > 0; remainingSpeed--)
            {
                var nextPosition = Point.GetPointInDirection(newPosition, direction);
                var nextCell = CurrentGame.Map.TryGetCell(nextPosition);
                if (nextCell == null)
                    return 0;
                
                if (nextCell.BlocksMovement)
                {
                    blockCell = nextCell;
                    return remainingSpeed;
                }

                var movementResult = MovementHelper.MoveObject(target, newPosition, nextPosition, false);
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
            var iceObject = cell.Objects.OfType<IIce>().FirstOrDefault();
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