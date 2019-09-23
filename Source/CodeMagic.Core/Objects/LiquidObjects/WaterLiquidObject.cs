using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Objects.SteamObjects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.LiquidObjects
{
    public interface IWaterLiquidObject : ILiquidObject, IInjectable
    {
    }

    public class WaterLiquidObject : AbstractLiquidObject, IWaterLiquidObject
    {
        public const string LiquidType = "WaterLiquid";
        
        public WaterLiquidObject(int volume) 
            : base(volume, LiquidType)
        {
        }

        public override string Name => "Water";

        protected override IIceObject CreateIce(int volume)
        {
            return Injector.Current.Create<IWaterIceObject>(volume);
        }

        protected override ISteamObject CreateSteam(int volume)
        {
            return Injector.Current.Create<IWaterSteamObject>(volume);
        }

        public override ISpreadingObject Separate(int volume)
        {
            Volume -= volume;
            return Injector.Current.Create<IWaterLiquidObject>(volume);
        }

        protected override void UpdateLiquid(IAreaMap map, IJournal journal, Point position)
        {
            base.UpdateLiquid(map, journal, position);

            var cell = map.GetCell(position);
            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                if (Volume < MinVolumeForEffect)
                    return;

                destroyable.Statuses.Remove(OnFireObjectStatus.StatusType);
                destroyable.Statuses.Remove(OilyObjectStatus.StatusType);

                destroyable.Statuses.Add(new WetObjectStatus(Configuration), journal);
            }
        }
    }
}