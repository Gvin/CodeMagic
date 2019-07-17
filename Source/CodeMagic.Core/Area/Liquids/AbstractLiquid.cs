using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area.Liquids
{
    public abstract class AbstractLiquid : ILiquid
    {
        private int volume;

        protected AbstractLiquid(int volume)
        {
            this.volume = volume;
        }

        protected abstract int FreezingPoint { get; }

        protected abstract int BoilingPoint { get; }

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

        public abstract int MaxVolume { get; }

        public abstract int MaxSpreadVolume { get; }

        public abstract ILiquid Separate(int separateVolume);

        public void Update(AreaMapCell cell)
        {
            if (Volume == 0)
                return;

            if (cell.Environment.Temperature <= FreezingPoint)
            {
                ProcessFreezing(cell);
                return;
            }

            if (cell.Environment.Temperature >= BoilingPoint)
            {
                ProcessBoiling(cell);
            }
        }

        protected virtual void ProcessBoiling(AreaMapCell cell)
        {
            // Do nothing
        }

        protected virtual void ProcessFreezing(AreaMapCell cell)
        {
            // Do nothing
        }

        public virtual void ApplyEffect(IDestroyableObject destroyable, Journal journal)
        {
            // Do nothing
        }
    }
}