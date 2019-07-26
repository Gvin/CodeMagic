namespace CodeMagic.Core.Objects
{
    public interface ISpreadingObject : IVolumeObject
    {
        int MaxVolumeBeforeSpread { get; }

        int MaxSpreadVolume { get; }

        ISpreadingObject Separate(int volume);
    }
}