using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public class WaterLiquidObject : AbstractLiquidObject<WaterIceObject>
    {
        public const string LiquidType = "water";
        private const string CustomValueWetStatusLifeTime = "WetStatusLifeTime";

        public WaterLiquidObject(int volume) 
            : base(volume, LiquidType)
        {
        }

        public override string Name => "Water";


        protected override WaterIceObject CreateIce(int volume)
        {
            return new WaterIceObject(volume);
        }

        public override ILiquidObject Separate(int volume)
        {
            Volume -= volume;
            return new WaterLiquidObject(volume);
        }

        protected override void UpdateLiquid(IGameCore game, Point position)
        {
            base.UpdateLiquid(game, position);

            var cell = game.Map.GetCell(position);
            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                if (Volume < MinVolumeForEffect)
                    return;

                if (destroyable.Statuses.Contains(OnFireObjectStatus.StatusType))
                {
                    destroyable.Statuses.Remove(OnFireObjectStatus.StatusType);
                }

                var lifeTime = GetWetStatusLifeTime();
                destroyable.Statuses.Add(new WetObjectStatus(lifeTime));
            }
        }

        private int GetWetStatusLifeTime()
        {
            var stringValue = GetCustomConfigurationValue(CustomValueWetStatusLifeTime);
            return int.Parse(stringValue);
        }
    }
}