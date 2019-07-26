using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Objects.SteamObjects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public class WaterLiquidObject : AbstractLiquidObject
    {
        public const string LiquidType = "WaterLiquid";
        
        public WaterLiquidObject(int volume) 
            : base(volume, LiquidType)
        {
        }

        public override string Name => "Water";

        protected override IIceObject CreateIce(int volume)
        {
            return MapObjectsFactory.CreateIceObject<WaterIceObject>(volume);
        }

        protected override ISteamObject CreateSteam(int volume)
        {
            return MapObjectsFactory.CreateSteam<WaterSteamObject>(volume);
        }

        public override ISpreadingObject Separate(int volume)
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