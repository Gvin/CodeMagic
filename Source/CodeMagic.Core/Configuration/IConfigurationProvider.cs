using CodeMagic.Core.Configuration.Liquids;

namespace CodeMagic.Core.Configuration
{
    public interface IConfigurationProvider
    {
        ISpellsConfiguration Spells { get; }

        IPhysicsConfiguration Physics { get; }

        ILiquidsConfiguration Liquids { get; }
    }
}