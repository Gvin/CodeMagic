namespace CodeMagic.Core.Objects.LiquidObjects
{
    public interface ILiquidObject : IMapObject, IDynamicObject
    {
        int Volume { get; set; }

        int MaxVolumeBeforeSpread { get; }

        int MaxSpreadVolume { get; }

        ILiquidObject Separate(int volume);
    }
}