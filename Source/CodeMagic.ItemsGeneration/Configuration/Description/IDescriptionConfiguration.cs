namespace CodeMagic.ItemsGeneration.Configuration.Description
{
    public interface IDescriptionConfiguration
    {
        IRarenessDescriptionConfiguration[] RarenessDescription { get; }

        IMaterialDescriptionConfiguration[] MaterialDescription { get; }
    }
}