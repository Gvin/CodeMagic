namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Tool
{
    public interface IToolsConfiguration
    {
        IToolConfiguration LumberjackAxe { get; }

        IToolConfiguration Pickaxe { get; }
    }
}