namespace CodeMagic.ItemsGeneration.Configuration.Tool
{
    public interface IToolsConfiguration
    {
        IToolConfiguration LumberjackAxe { get; }

        IToolConfiguration Pickaxe { get; }
    }
}