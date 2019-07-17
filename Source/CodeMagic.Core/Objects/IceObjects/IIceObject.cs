namespace CodeMagic.Core.Objects.IceObjects
{
    public interface IIceObject : IMapObject, IDynamicObject, IStepReactionObject
    {
        int Volume { get; set; }

        bool SupportsSlide { get; }
    }
}