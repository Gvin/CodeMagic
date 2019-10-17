namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Description
{
    public interface IDescriptionConfiguration
    {
        IRarenessDescriptionConfiguration[] RarenessDescription { get; }

        IMaterialDescriptionConfiguration[] MaterialDescription { get; }
    }
}