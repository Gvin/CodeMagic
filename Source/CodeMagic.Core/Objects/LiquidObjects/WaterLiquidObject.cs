using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public class WaterLiquidObject : AbstractLiquidObject<WaterIceObject>
    {
        public const string LiquidType = "water";
        
        public WaterLiquidObject(int volume) 
            : base(volume, LiquidType)
        {
        }

        public override string Name => "Water";


        protected override WaterIceObject CreateIce(int volume)
        {
            return MapObjectsFactory.CreateIceObject<WaterIceObject>(volume);
        }

        public override ILiquidObject Separate(int volume)
        {
            Volume -= volume;
            return MapObjectsFactory.CreateLiquidObject<WaterLiquidObject>(volume);
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

                destroyable.Statuses.Remove(OnFireObjectStatus.StatusType);
                destroyable.Statuses.Remove(OilyObjectStatus.StatusType);

                destroyable.Statuses.Add(new WetObjectStatus(Configuration));
            }
        }
    }
}