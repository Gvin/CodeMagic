namespace CodeMagic.Core.Objects
{
    public interface IVolumeObject : IMapObject
    {
        string Type { get; }

        int Volume { get; set; }
    }
}